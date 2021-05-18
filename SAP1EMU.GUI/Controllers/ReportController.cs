using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SAP1EMU.GUI.Models;
using UAParser;
using Octokit;
using Microsoft.Extensions.Configuration;

namespace SAP1EMU.GUI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ReportController : ControllerBase
    {
        private readonly GitHubClient _githubClient;

        private static readonly string bugTag = "bug";
        private static readonly string featureTag = "enhancement";

        public ReportController(IConfiguration configuration)
        {
            _githubClient = new GitHubClient(new ProductHeaderValue("SAP1EMU"))
            {
                Credentials = new Credentials(configuration.GetValue<string>("GitHubIssueToken"))
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBugReport([FromForm] BugReport bugReport)
        {
            Parser uaParser = Parser.GetDefault();
            ClientInfo clientBrowser = uaParser.Parse(Request.Headers["User-Agent"].ToString());

            bugReport.BrowserInfo = $"{clientBrowser.UA.Family} Version {clientBrowser.UA.Major}.{clientBrowser.UA.Minor}";
            bugReport.SAP1EMUVersion = GetType().Assembly.GetName().Version.ToString();

            StringBuilder body = new StringBuilder();

            body.AppendLine("**SAP1EMU Version**");
            body.AppendLine($"v{bugReport.SAP1EMUVersion}");
            body.AppendLine();

            body.AppendLine("**Browser Info**");
            body.AppendLine(bugReport.BrowserInfo);
            body.AppendLine();

            body.AppendLine("**Description**");
            body.AppendLine(bugReport.Description);
            body.AppendLine();

            body.AppendLine("**Steps to reproduce bug**");
            body.AppendLine(bugReport.ReproductionSteps);

            try
            {
                Issue createdIssue = await CreateGithubIssue(bugReport.Title, body.ToString(), bugTag);

                return Ok(createdIssue);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }
            catch(AuthorizationException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeatureRequest([FromForm] FeatureRequest featureRequest)
        {
            featureRequest.SAP1EMUVersion = GetType().Assembly.GetName().Version.ToString();

            StringBuilder body = new StringBuilder();

            body.AppendLine("**SAP1EMU Version**");
            body.AppendLine($"v{featureRequest.SAP1EMUVersion}");
            body.AppendLine();

            body.AppendLine("**Description**");
            body.AppendLine(featureRequest.Description);

            try
            {
                Issue createdIssue = await CreateGithubIssue(featureRequest.Title, body.ToString(), featureTag);

                return Ok(createdIssue);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (AuthorizationException)
            {
                return Unauthorized();
            }
        }

        private async Task<Issue> CreateGithubIssue(string title, string body, string tag)
        {
            NewIssue issue = new NewIssue(title)
            {
                Body = body.ToString()
            };

            issue.Labels.Add(tag);

            return await _githubClient.Issue.Create("rbaker26", "SAP1EMU", issue);
        }
    }
}
