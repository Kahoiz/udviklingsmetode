using Benchmarker;
using System.Diagnostics;

class Program
{
    private static readonly string _url = "http://localhost:5000/Fibunachi/15";
    static async Task Main(string[] args)
    {
        try
        {
            //var benchmark = BenchmarkV1.RunBenchmarkAsync(_url,10000,50000);
            //var benchmark = BenchmarkV0.BenchmarkAsync();
            //var benchmark = BenchmarkV2.RunBenchmarkAsync(1000, _url);
            //var benchmark = BenchmarkV3.RunBenchmarkAsync(1000, _url);
            var benchmark = BenchmarkV4.RunBenchmark(_url, 1, 1, 1000);
            await benchmark;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
