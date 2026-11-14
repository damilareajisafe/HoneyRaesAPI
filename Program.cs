using HoneyRaesAPI.Models;

List<Customer> customers = new List<Customer>()
{
    new Customer()
    {
        Id = 1,
        Name = "John  Smith",
        Address = "12 Allen Avenue, Ikeja"
    },
    new Customer()
    {
        Id = 2,
        Name = "Mary Johnson",
        Address = "5 Admiralty Way, Lekki"
    },
    new Customer()
    {
        Id = 3,
        Name = "David Brown",
        Address = "20 Awolowo Road, Ikoyi"
    }
};

List<Employee> employees = new List<Employee>()
{
    new Employee()
    {
        Id = 1,
        Name = "Michael James",
        Specialty = "Electrical Repairs"
    },
    new Employee()
    {
        Id = 2,
        Name = "Sarah Williams",
        Specialty = "Plumbing"
    }
};

List<ServiceTicket> serviceTickets = new List<ServiceTicket>()
{
    new ServiceTicket()
    {
        Id = 1,
        CustomerId = 1,
        EmployeeId = 1,
        Description = "Faulty electrical wiring",
        Emergency = true,
        DateCompleted = new DateTime(2026, 10, 5)
    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 2,
        EmployeeId = 2,
        Description = "Leaking kitchen sink",
        Emergency = false,
        DateCompleted = new DateTime(2026, 10, 7)
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 3,
        EmployeeId = 1,
        Description = "Faulty circuit breaker",
        Emergency = true,
        DateCompleted = new DateTime(2026, 10, 9)
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 1,
        EmployeeId = 2,
        Description = "Blocked bathroom drain",
        Emergency = false,
        DateCompleted = new DateTime(2026, 10, 12)
    },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 2,
        EmployeeId = 1,
        Description = "Damaged electrical socket",
        Emergency = false,
        DateCompleted = new DateTime(2026, 10, 15)
    }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/hello", () =>
{
    return "hello";
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
