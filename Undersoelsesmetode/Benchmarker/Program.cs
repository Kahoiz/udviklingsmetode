using System.Diagnostics;

class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _url = "http://localhost:5000/Fubunachi/15";

    static async Task Main()
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