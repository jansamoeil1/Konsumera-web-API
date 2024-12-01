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

    // Metod för att hämta och visa repos från GitHub
    static async Task GetGitHubRepositories()
    {
        // GitHub API URL för .NET Foundation repositories
        var url = "https://api.github.com/orgs/dotnet/repos";

        // Skapa en HttpClient instans
        using var client = new HttpClient();

        // Ställ in User-Agent headern (GitHub kräver denna)
        client.DefaultRequestHeaders.Add("User-Agent", "DotNetHttpClient");

        try
        {
            // Skicka GET-förfrågan
            var response = await client.GetStringAsync(url);

            // Deserialisera JSON-data till en lista av Repository-objekt
            var repositories = JsonSerializer.Deserialize<Repository[]>(response);

            // Visa resultatet
            foreach (var repo in repositories)
            {
                // Formatera PushedAt datum till önskat format
                var pushedAt = repo.PushedAt.ToString("yyyy-MM-dd HH:mm:ss");

                // Hantera tomma värden för homepage och description
                var homepage = string.IsNullOrEmpty(repo.Homepage) ? "" : repo.Homepage;
                var description = string.IsNullOrEmpty(repo.Description) ? "" : repo.Description;

                // Skriv ut informationen för varje repo i exakt format
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

// C#-klass för att representera JSON-datan från GitHub API
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