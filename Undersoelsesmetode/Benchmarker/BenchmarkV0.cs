using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarker
{
    internal class BenchmarkV0
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly string _url = "http://localhost:5000/Fibunachi/15";
        public static async Task BenchmarkAsync()
        {
            int total = 1000000;
            int concurrent = 1000;

            var stopwatch = Stopwatch.StartNew();


            for (int i = 0; i < total; i += concurrent)
            {
                var bundlewatch = Stopwatch.StartNew();
                var tasks = new Task<HttpResponseMessage>[concurrent];

                for (int j = 0; j < concurrent; j++)
                {
                    if (i + j >= total) break;

                    tasks[j] = SendRequest(i + j);
                }

                await Task.WhenAll(tasks);
                bundlewatch.Stop();
                Console.WriteLine($"Time for bundle {bundlewatch.ElapsedMilliseconds} ms");

                if(bundlewatch.ElapsedMilliseconds % 1000 > 0)
                {
                    Console.WriteLine($"Waiting for {1000 - (int)bundlewatch.ElapsedMilliseconds % 1000} ms");  
                    await Task.Delay(1000 - (int)bundlewatch.ElapsedMilliseconds % 1000);
                }
            }


            stopwatch.Stop();
            Console.WriteLine($"Total time for {total} requests: {stopwatch.ElapsedMilliseconds} ms");
        }

        private static async Task<HttpResponseMessage> SendRequest(int requestId)
        {
            var response = await _httpClient.GetAsync(_url);

            return response;
        }
    }
}
