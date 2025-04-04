using System.Diagnostics;
using System.Net.Http;

namespace Benchmarker;

internal class BenchmarkV4
{
    private static readonly HttpClient _httpClient = new();

    public static async Task RunBenchmark(string url, int initialRate = 1,
                                   int increaseRate = 1,
                                   int maxRequests = 100)
                                   
    {
        int currentRate = initialRate;
        Stopwatch stopwatch = new();
        while (currentRate <= maxRequests)
        {
            Console.WriteLine($"Sending {currentRate} requests...");
            
            stopwatch.Restart();
            // Multi-threaded execution
            List<Task<HttpResponseMessage>> tasks = new();

            for(int i = 0; i < currentRate; i++)
            {
                tasks.Add(SendRequest());
            }

            Task.WhenAll(tasks).Wait();

            Console.WriteLine($"Time taken for {currentRate} requests: {stopwatch.ElapsedMilliseconds} ms");

            // Check if request time exceeds 1 second
            if (stopwatch.ElapsedMilliseconds < 1000)
            {
                //only increase the rate if the time is less than 1 second
                currentRate += increaseRate; // Increase rate as per y = ax + b
                Console.WriteLine($"Increased rate to {currentRate} requests.");

                await Task.Delay(1000 - (int)stopwatch.ElapsedMilliseconds); // Wait until the next cycle
            }
                        
        }
    }

    private static async Task<HttpResponseMessage> SendRequest()
    {
        var response = await _httpClient.GetAsync("http://localhost:5000/Fibunachi/41");
        return response;
    }
}