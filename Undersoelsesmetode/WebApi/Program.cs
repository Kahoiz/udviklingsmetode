using Microsoft.Extensions.Logging;
using MathLib;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.MapGet("/fibunacci/{number:int}", async (int number) =>
{
    app.MapGet("/fibunacci/{number:int}", async (int number) =>
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();

        // Capture initial memory and CPU usage
        long initialMemory = process.WorkingSet64;
        TimeSpan initialCpuTime = process.TotalProcessorTime;

        // Perform the Fibonacci calculation
        int result = await Task.Run(() => Fibunachi.GetFibunachi(number));

        // Capture final memory and CPU usage
        long finalMemory = process.WorkingSet64;
        TimeSpan finalCpuTime = process.TotalProcessorTime;

        // Calculate the differences
        long memoryUsed = finalMemory - initialMemory;
        TimeSpan cpuTimeUsed = finalCpuTime - initialCpuTime;

        // Return the result along with resource usage
        return Results.Ok(new
        {
            Message = $"Fibunacci number {number} = {result}",
            MemoryUsedInBytes = memoryUsed,
            CpuTimeUsedInMilliseconds = cpuTimeUsed.TotalMilliseconds
        });
    });
});

app.MapGet("/linear/{number:int}", async (int number) =>
{
    List<int> res = await Task.Run(() => LinearTask.CalculateLinearFunction(number));
    return Results.Ok(res);
});


app.Run();
