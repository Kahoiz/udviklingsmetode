﻿using System.Diagnostics;

namespace Benchmarker;

public class BenchmarkV6
{
    private static readonly HttpClient client = new();
    private static readonly string _url = "http://localhost:5000/fibunacci/41";
    private static readonly int _requestsPerSecond = 7;
    private static int _threadCounter = 0;
    private static readonly string _csvFilePath = "benchmark_results.csv";
    private static readonly object _fileLock = new();

    public static void Run()
    {
        using (var writer = new StreamWriter(_csvFilePath, false))
        {
            writer.WriteLine("ThreadId;RequestsCount;ElapsedMilliseconds");
        }

        Task.Run(() => SendRequestsContinuously()).Wait();
    }

    private static async Task SendRequestsContinuously()
    {
        while (true)
        {
            var thread = new Thread(new ThreadStart(RunBenchmark));
            thread.Start();

            await Task.Delay(1000);
        }
    }

    private static async Task<HttpResponseMessage> SendRequest()
    {
        return await client.GetAsync(_url);
    }

    private static async void RunBenchmark()
    {
        int threadId = Interlocked.Increment(ref _threadCounter);
        Stopwatch watch = Stopwatch.StartNew();
        List<Task<HttpResponseMessage>> tasks = new();

        for (int i = 0; i < _requestsPerSecond; i++)
        {
            tasks.Add(SendRequest());
        }

        await Task.WhenAll(tasks);
        watch.Stop();

        Console.WriteLine($"Thread {threadId} completed {tasks.Count} requests in {watch.ElapsedMilliseconds} ms");


        lock (_fileLock)
        {
            using (var writer = new StreamWriter(_csvFilePath, true))
            {
                writer.WriteLine($"{threadId};{tasks.Count};{watch.ElapsedMilliseconds}");
            }
        }
    }
}