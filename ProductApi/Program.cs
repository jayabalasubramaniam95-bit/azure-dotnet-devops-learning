var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/api/products/{id}", (int id) =>
{
    var products = new[]
    {
        new { Id = 1, Name = "Laptop", Price = 1200 },
        new { Id = 2, Name = "Monitor", Price = 450 },
        new { Id = 3, Name = "Keyboard", Price = 100 }
    };

    var product = products.FirstOrDefault(p => p.Id == id);

    return product is not null
        ? Results.Ok(product)
        : Results.NotFound();
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
