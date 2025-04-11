using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker;
internal class LinearBenchmarkerV0
{
    private static readonly HttpClient _httpClient = new();
    private static readonly string _url = "http://localhost:5000/linear/";

    public static async Task RunBenchmark()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        int x = 0,
            requestAmount = 1;
        List<Task<HttpResponseMessage>> tasks = new();

        while (requestAmount < 10000)
        {
            stopwatch.Restart();
            for (int i = 0; i < requestAmount; i++)
            {

                tasks.Add(SendRequest(_url + x));
                x++;

            }
            await Task.WhenAll(tasks);
            Console.WriteLine($"Requests made: {requestAmount} | Time taken: {stopwatch.ElapsedMilliseconds}ms | x: {x}");

            // Check if request time exceeds 1 second
            // If true, then we wait for next cycle
            if (stopwatch.ElapsedMilliseconds < 1000)
            {
                await Task.Delay(1000 - (int)stopwatch.ElapsedMilliseconds); // Wait until the next cycle
            }

            // Increase the request amount
            requestAmount++;
        }
        stopwatch.Stop();
        Console.WriteLine($"Total requests made: {x}");
    }

    private static async Task<HttpResponseMessage> SendRequest(string url)
    {
        var response = await _httpClient.GetAsync(url);
        return response;
    }
}
