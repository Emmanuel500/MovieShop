using System.Diagnostics;

namespace MovieShopMVC
{
    public class AsynchTest
    {
        static readonly HttpClient s_client = new HttpClient
        {
            MaxResponseContentBufferSize = 1_000_000
        };

        static readonly IEnumerable<string> s_urlList = new string[]
        {
            "https://localhost:7085/Movies/MoviesByGenre/1",
            "https://localhost:7085/Movies/MoviesByGenre/5",
            "https://localhost:7085/Movies/MoviesByGenre/10",
            "https://localhost:7085/Movies/MoviesByGenre/15",
            "https://localhost:7085/Movies/MoviesByGenre/20"
        };

        static Task Main() => SumPageSizesAsync();

        static async Task SumPageSizesAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            IEnumerable<Task<int>> downloadTasksQuery =
                from url in s_urlList
                select ProcessUrlAsync(url, s_client);

            List<Task<int>> downloadTasks = downloadTasksQuery.ToList();

            int total = 0;
            while (downloadTasks.Any())
            {
                Task<int> finishedTask = await Task.WhenAny(downloadTasks);
                downloadTasks.Remove(finishedTask);
                total += await finishedTask;
            }

            stopwatch.Stop();

            Console.WriteLine($"\nTotal bytes returned:  {total:#,#}");
            Console.WriteLine($"Elapsed time:          {stopwatch.Elapsed}\n");
        }

        static async Task<int> ProcessUrlAsync(string url, HttpClient client)
        {
            byte[] content = await client.GetByteArrayAsync(url);
            Console.WriteLine($"{url,-60} {content.Length,10:#,#}");

            return content.Length;
        }
    }
}
