using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        await GetGitHubRepositories();
    }

    static async Task GetGitHubRepositories()
    {
        var url = "https://api.github.com/orgs/dotnet/repos";

        using var client = new HttpClient();

        client.DefaultRequestHeaders.Add("User-Agent", "DotNetHttpClient");

        try
        {
            var response = await client.GetStringAsync(url);

            var repositories = JsonSerializer.Deserialize<Repository[]>(response);

            foreach (var repo in repositories)
            {
                var pushedAt = repo.PushedAt.ToString("yyyy-MM-dd HH:mm:ss");

                var homepage = string.IsNullOrEmpty(repo.Homepage) ? "" : repo.Homepage;
                var description = string.IsNullOrEmpty(repo.Description) ? "" : repo.Description;

                Console.WriteLine($"Name: {repo.Name}");
                Console.WriteLine($"Homepage: {homepage}");
                Console.WriteLine($"GitHub: {repo.HtmlUrl}");
                Console.WriteLine($"Description: {description}");
                Console.WriteLine($"Watchers: {repo.Watchers:n0}");
                Console.WriteLine($"Last push: {pushedAt}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}


public class Repository
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; }

    [JsonPropertyName("watchers")]
    public int Watchers { get; set; }

    [JsonPropertyName("pushed_at")]
    public DateTime PushedAt { get; set; }
}