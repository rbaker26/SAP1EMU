using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;      // TODO: Replace with System.Text.Json

using SAP1EMU.Data.Lib.Security;
using SAP1EMU.GUI.Contexts;


// Credit to Hemant Manwani - August 26, 2020
// URL: https://www.loginradius.com/blog/async/anti-xss-middleware-asp-core/

namespace SAP1EMU.GUI.Middleware
{
    public class AntiXssMiddleware
    {
        private readonly RequestDelegate _next;
        private ErrorResponse _error;
        private readonly int _statusCode = (int)HttpStatusCode.BadRequest;
        private Sap1EmuContext _sap1EmuContext { get; set; } = null;


        public AntiXssMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context, Sap1EmuContext sap1EmuContext)
        {
            
            _sap1EmuContext = sap1EmuContext;


            // Check XSS in URL
            if (!string.IsNullOrWhiteSpace(context.Request.Path.Value))
            {
                var url = context.Request.Path.Value;

                if (CrossSiteScriptingValidation.IsDangerousString(url, out _))
                {
                    await RespondWithAnError(context).ConfigureAwait(false);
                    return;
                }
            }

            // Check XSS in query string
            if (!string.IsNullOrWhiteSpace(context.Request.QueryString.Value))
            {
                var queryString = WebUtility.UrlDecode(context.Request.QueryString.Value);

                if (CrossSiteScriptingValidation.IsDangerousString(queryString, out _))
                {
                    await RespondWithAnError(context).ConfigureAwait(false);
                    return;
                }
            }

            // Check XSS in request content
            var originalBody = context.Request.Body;
            try
            {
                var content = await ReadRequestBody(context);

                if (CrossSiteScriptingValidation.IsDangerousString(content, out _))
                {
                    await RespondWithAnError(context).ConfigureAwait(false);
                    return;
                }
                await _next(context).ConfigureAwait(false);
            }
            finally
            {
                context.Request.Body = originalBody;
            }
        }

        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            var buffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(buffer);
            context.Request.Body = buffer;
            buffer.Position = 0;

            var encoding = Encoding.UTF8;

            var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return requestContent;
        }

        private async Task RespondWithAnError(HttpContext context)
        {
            // Log the threat
            using (MemoryStream ms = new MemoryStream(2048))
            {
                await context.Request.Body.CopyToAsync(ms);
                ms.Position = 0;
                ThreatLog threatLog = new ThreatLog()
                {
                    ThreatIdentifier = Guid.NewGuid(),
                    ClientIpAddress = context.Connection.RemoteIpAddress.ToString(),
                    Url = context.Request.Host + context.Request.Path,
                    QueryString = context.Request.QueryString.ToString(),
                    ThreatContent = AntiXss.Sanitize(await new StreamReader(ms, Encoding.UTF8).ReadToEndAsync()),
                    ThreatTypeId = ThreatFactory.GetThreat(ThreatType.Xss).Id
                };
                _sap1EmuContext.ThreatLogs.Add(threatLog);
                _sap1EmuContext.SaveChanges();
            }
                

            // Send Client-Side Response
            context.Response.Clear();
            context.Response.Headers.AddHeaders();
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = _statusCode;

            if (_error == null)
            {
                _error = new ErrorResponse
                {
                    Description = "XSS Detected",
                    ErrorCode = 500,
                    ThreatInfo = new ThreatInfo
                    {
                        Action = "Incident Recorded",
                        ClientIp = context.Connection.RemoteIpAddress.ToString()
                    }
                };
            }

            await context.Response.WriteAsync(_error.ToJSON());
        }
    }

    public static class AntiXssMiddlewareExtension
    {
        public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AntiXssMiddleware>();
        }
    }


    /// <summary>
    /// Imported from System.Web.CrossSiteScriptingValidation Class
    /// </summary>
    public static class CrossSiteScriptingValidation
    {
        private static readonly char[] StartingChars = { '<', '&' };

        public static bool IsDangerousString(string s, out int matchIndex)
        {
            //bool inComment = false;
            matchIndex = 0;

            for (var i = 0; ;)
            {

                // Look for the start of one of our patterns 
                var n = s.IndexOfAny(StartingChars, i);

                // If not found, the string is safe
                if (n < 0) return false;

                // If it's the last char, it's safe 
                if (n == s.Length - 1) return false;

                matchIndex = n;

                switch (s[n])
                {
                    case '<':
                        // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                        if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?') return true;
                        break;
                    case '&':
                        // If the & is followed by a #, it's unsafe (e.g. S) 
                        if (s[n + 1] == '#') return true;
                        break;

                }

                // Continue searching
                i = n + 1;
            }
        }

        private static bool IsAtoZ(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public static void AddHeaders(this IHeaderDictionary headers)
        {
            if (headers["P3P"].IsNullOrEmpty())
            {
                headers.Add("P3P", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
        public static string ToJSON(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }

    public static class AntiXss
    {
        public static string Sanitize(string s)
        {
            return s.Replace("<", "&gt;").Replace(">", "&lt;");
        }
    }

    public class ErrorResponse
    {
        public int ErrorCode { get; set; }
        public string Description { get; set; }

        public ThreatInfo ThreatInfo { get; set; }

    }

    public class ThreatInfo
    {
        public string Action { get; set; }
        public string ClientIp { get; set; }
    }
}
