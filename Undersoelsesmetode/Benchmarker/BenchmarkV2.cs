
using System.Diagnostics;
using System.Text;

namespace Benchmarker;
internal class BenchmarkV2
{
    public static async Task RunBenchmarkAsync(int requestCount, string url)
    {
        using (HttpClient client = new HttpClient())
        {
            Stopwatch stopwatch = new Stopwatch();
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("RequestNumber,StatusCode,ResponseTime(ms)");

            for (int i = 0; i < requestCount; i++)
            {
                stopwatch.Restart();
                HttpResponseMessage response = await client.GetAsync(url);
                stopwatch.Stop();
                double responseTime = stopwatch.Elapsed.TotalMilliseconds;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Request {i + 1} failed with status code: {response.StatusCode}");
                }

                csvContent.AppendLine($"{i + 1};{response.StatusCode};{responseTime}");
                Console.WriteLine($"Request #{i + 1} | Status Code: {response.StatusCode} | Response Time: {responseTime}ms");
            }
            
            File.WriteAllText("benchmark_results.csv", csvContent.ToString());
        }
    }
}
