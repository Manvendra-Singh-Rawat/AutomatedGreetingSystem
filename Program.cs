using AutomatedGreetingSystem.Application.Interfaces;
using AutomatedGreetingSystem.Application.Services;
using AutomatedGreetingSystem.Infrastructure.GreetingService;
using AutomatedGreetingSystem.Infrastructure.Persistence.PostgreSQL;
using AutomatedGreetingSystem.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// postgres database connection
builder.Services.AddDbContext<AutoGreetDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IGreetingService, GreetingService>();

builder.Services.AddHostedService<GreetingBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AutoGreetDbContext>();
    db.Database.Migrate();
}

app.Run();
