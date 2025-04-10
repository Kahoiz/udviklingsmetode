using Benchmarker;
using System.Diagnostics;

class Program
{
    private static readonly string _urldotnet = "http://localhost:5000/fibunacci/21";
    private static readonly string _urlphp = "http://localhost:8080/api/21";
    static async Task Main(string[] args)
    {
        try
        {
            //var benchmark = BenchmarkV1.RunBenchmarkAsync(_url,10000,50000);
            //var benchmark = BenchmarkV0.BenchmarkAsync();
            //var benchmark = BenchmarkV2.RunBenchmarkAsync(1000, _url);
            //var benchmark = BenchmarkV3.RunBenchmarkAsync(1000, _url);
            
            var benchmark = BenchmarkV4.RunBenchmark(_urldotnet, "dotnet-no-optimize", 1, 1, 100);
            //BenchmarkV6.Benchmark();

            //var benchmark = LinearBenchmarkerV0.RunBenchmark();
            //var benchmark = LinearBenchmarkerV1.RunBenchmark();
            await benchmark;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
