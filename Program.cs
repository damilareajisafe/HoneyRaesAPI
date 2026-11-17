using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;
using System.Collections.Immutable;

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
        Description = "Faulty circuit breaker",
        Emergency = true,
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 1,
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

app.MapGet("/servicetickets", () =>
{
    return serviceTickets.Select(t => new ServiceTicketDTO
    {
        Id = t.Id,
        CustomerId = t.CustomerId,
        EmployeeId = t.EmployeeId,
        Description = t.Description,
        Emergency = t.Emergency,
        DateCompleted = t.DateCompleted
    });
});

app.MapGet("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(st => st.Id == id);

    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    Employee employee = employees.FirstOrDefault(e => e.Id == serviceTicket.EmployeeId);
    Customer customer = customers.FirstOrDefault(c => c.Id == serviceTicket.CustomerId);

    return Results.Ok(new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        EmployeeId = serviceTicket.EmployeeId,
        Employee = employee == null ? null : new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Specialty = employee.Specialty
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    });
});

app.MapGet("/employees", () =>
{
    return employees.Select(e => new EmployeeDTO()
    {
        Id = e.Id,
        Name = e.Name,
        Specialty = e.Specialty
    });
});

app.MapGet("/employees/{id}", (int id) =>
{
    Employee employee = employees.FirstOrDefault(e => e.Id == id);

    if (employee == null)
    {
        return Results.NotFound();
    }

    List<ServiceTicket> tickets = serviceTickets.Where(st => st.EmployeeId == id).ToList();

    return Results.Ok(new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO()
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});

app.MapGet("/customers", () =>
{
    return customers.Select(c => new CustomerDTO()
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address
    });
});

app.MapGet("/customers/{id}", (int id) =>
{
    Customer customer = customers.FirstOrDefault(c => c.Id == id);

    if (customer == null)
    {
        return Results.NotFound();
    }

    List<ServiceTicket> tickets = serviceTickets.Where(st => st.CustomerId == id).ToList();

    return Results.Ok(new CustomerDTO()
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        ServiceTickets = tickets.Select(t => new ServiceTicketDTO
        {
            Id = t.Id,
            CustomerId = t.CustomerId,
            EmployeeId = t.EmployeeId,
            Description = t.Description,
            Emergency = t.Emergency,
            DateCompleted = t.DateCompleted
        }).ToList()
    });
});


app.Run();

