using System.Diagnostics;

namespace Benchmarker;

public static class BenchmarkV5
{
    
    private static readonly HttpClient client = new();
    private static readonly string _url = "http://localhost:5000/fibunacci/15";
    private static readonly int _threadCount = 10;
    private static readonly int _bundleSize = 900;

    public static void Run()
    {
        List<Thread> threads = new();

        for (int i = 0; i < _threadCount; i++)
        {
            Thread thread = new Thread(new ThreadStart(RunBenchmark));
            threads.Add(thread);
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        Console.WriteLine("All threads completed.");
    }
    
    private static void RunBenchmark()
    {
        Stopwatch watch = Stopwatch.StartNew();
        List<Task<HttpResponseMessage>> tasks = new();

        for (int i = 0; i < _bundleSize; i++)
        {
            tasks.Add(SendRequest());
        }
        
        Task.WaitAll(tasks);
        watch.Stop();
        
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} completed {tasks.Count} requests in {watch.ElapsedMilliseconds} ms");
        
    }
    private static async Task<HttpResponseMessage> SendRequest()
    {
        return await client.GetAsync(_url);
    }
}