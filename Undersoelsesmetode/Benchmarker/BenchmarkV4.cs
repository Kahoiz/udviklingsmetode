using System.Diagnostics;
using System.Net.Http;

namespace Benchmarker;

internal class BenchmarkV4
{
    private static readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30) // Set a reasonable timeout for requests
    };

    public static async Task RunBenchmark(string url, string env, int initialRate = 1,
                                   int increaseRate = 1,
                                   int maxRequests = 100)
    {
        int currentRate = initialRate;
        Stopwatch stopwatch = new();
        List<Task<HttpResponseMessage>> tasks = new(); // Fixed initialization of the list
        DateTime batchStart = DateTime.UtcNow;
        while (currentRate <= maxRequests)
        {
            Console.WriteLine($"Sending {currentRate} requests...");

            stopwatch.Restart();

            // Multi-threaded execution
            tasks.Clear();

            for (int i = 0; i < currentRate; i++)
            {
                tasks.Add(SendRequest(url));
            }

            batchStart = DateTime.Now;
            await Task.WhenAll(tasks); // Use await instead of blocking with .Wait()

            Console.WriteLine($"Time taken for {currentRate} requests: {stopwatch.ElapsedMilliseconds} ms");

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds < 1000)
            {
                int delay = 1000 - (int)stopwatch.ElapsedMilliseconds;
                if (delay > 0)
                {
                    await Task.Delay(delay); // Avoid negative delay
                }
            }

            currentRate += increaseRate;
            Console.WriteLine($"Increased rate to {currentRate} requests.");




            // Log results to CSV
            LogResults(env,batchStart,stopwatch.ElapsedMilliseconds, currentRate);
        }
    }

    private static void LogResults(string env,DateTime batchStart, long batchTime, int requestPrSecound)
    {
        string filePath = $"{env}_benchmark_results_{DateTime.UtcNow:yyyyMMdd}.csv";

        // Ensure the file has a header if it doesn't exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "BatchStart;BatchTime-ms;Batchsize\n");
        }

        // Append results to the CSV file
        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine($"{batchStart:yyyy-MM-dd HH:mm:ss};{batchTime};{requestPrSecound}");
        }

        Console.WriteLine($"Results logged to {filePath}");
    }

    private static async Task<HttpResponseMessage> SendRequest(string url)
    {
        var response = await _httpClient.GetAsync(url);

        return response;
    }
}

    