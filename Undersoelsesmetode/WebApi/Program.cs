using Microsoft.Extensions.Logging;
using MathLib;

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

app.MapGet("/Fibunachi/{number:int}", async (int number) =>
{
    try
    {
        if (number < 0)
        {
            return Results.BadRequest("Number must be non-negative.");
        }

        int result = await Task.Run(() => Fibunachi.GetFibunachi(number));
        return Results.Ok($"Fibunachi number {number} = {result}");
    }
    catch (Exception ex)
    {
        // Log the exception
        logger.LogError(ex, "An error occurred while processing the request.");
        return Results.Problem("An error occurred while processing your request.");
    }
});

app.Run();
