using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SAP1EMU.GUI.Models
{
    public class GitHubProfileModel
    {
        public static async Task<GitHubProfileModel> GetProfile(string username)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "SAP1Emu WebApp");
            var result = await httpClient.GetAsync($"https://api.github.com/users/{username}");
            return JsonSerializer.Deserialize<GitHubProfileModel>(await result.Content.ReadAsStringAsync());
        }

        public string login { get; set; }
        public string avatar_url { get; set; }
        public string html_url { get; set; }
        public string followers_url { get; set; }
        public string following_url { get; set; }
        public string name { get; set; }
        public string bio { get; set; }
        public int public_repos { get; set; }
        public int followers { get; set; }
    }
}