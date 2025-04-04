using Benchmarker;
using System.Diagnostics;

class Program
{
    private static readonly string _url = "http://localhost:5000/Fubunachi/15";
    static async Task Main(string[] args)
    {
        //var benchmark = BenchmarkV1.RunBenchmarkAsync(_url,10000,50000);
        //var benchmark = BenchmarkV0.BenchmarkAsync();
        BenchmarkV6.Run();
    }
}
