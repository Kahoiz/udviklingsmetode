using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker;

internal class BenchmarkV1
{
    public static async Task RunBenchmarkAsync(string apiUrl,
                                               int requestsPerSecond,
                                               int totalRequests)
    {
        int requestsSent = 0;

        using (HttpClient client = new())
        {
            Stopwatch stopwatch = new();

            while (requestsSent < totalRequests)
            {
                stopwatch.Restart();
                try
                {
                    // Send GET request
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    stopwatch.Stop();

                    // Get response time
                    double responseTime = stopwatch.Elapsed.TotalMilliseconds;

                    // Log response time
                    Console.WriteLine($"Request #{requestsSent + 1} | Response Time: {responseTime}ms");

                    // Check if response time exceeds the allowed time (e.g., 1000ms per request)
                    if (responseTime > 1000)  // If the request is too slow
                    {
                        Console.WriteLine($"Warning: Request took too long ({responseTime}ms). Adjusting request rate...");
                        await Task.Delay(1000 - (int)responseTime); // Wait until the next cycle
                    }

                    requestsSent++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                // Ensure requests are spaced to maintain the desired rate (e.g., 10 requests per second)
                if (requestsSent % requestsPerSecond == 0)
                {
                    await Task.Delay(1000); // Delay to maintain requests per second rate
                }
            }
        }

        Console.WriteLine("Test Completed.");
    }
}
   