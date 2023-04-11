using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add database context
builder.Services.AddDbContext<FactoryDeviceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// Add services
builder.Services.AddScoped<IFactoryDeviceService, FactoryDeviceService>();

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

app.UseAuthorization();

app.MapControllers();

// Migrate the database on startup (apply any pending migrations)
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<FactoryDeviceDbContext>();
dbContext.Database.EnsureCreated();

// Seed data handling
using (var currentScope = app.Services.CreateScope())
{
    var serviceProvider = currentScope.ServiceProvider;

    SeedData.Initialize(serviceProvider);
}

app.Run();