using System.Diagnostics;

namespace Benchmarker;

internal class BenchmarkV3
{
    private static readonly HttpClient _httpClient = new();
    public static async void BenchmarkAsync()
    {
        int requestsPerSecond = 1000;
        int totalRequests = 1000000;
        int requestsSent = 0;
        int amountOfBundles = 1;

        List<Task<long>> tasks = new();
        
        while (requestsSent < totalRequests)
        {
            long totalTime = 0;

            for (int i = 0; i < amountOfBundles; i++)
            {
                tasks.Add(SendBundleRequest());
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                totalTime += task.Result;
                Console.WriteLine($"Bundle time: {task.Result} ms");
            }
        }
    }

    private static async Task<long> SendBundleRequest()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        List<Task<HttpResponseMessage>> tasks = [];

        for (int i = 0; i < 1000; i++)
        {
            tasks.Add(SendRequest());
        }

        await Task.WhenAll(tasks);

        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    private static async Task<HttpResponseMessage> SendRequest()
    {
        var response = await _httpClient.GetAsync("http://localhost:5000/Fibunachi/15");
        return response;
    }
}

