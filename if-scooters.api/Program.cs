using if_scooters.core.Models;
using if_scooters.core.Services;
using if_scooters.data;
using if_scooters.services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("if-scooters");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ScooterDbContext>(x => x.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IScooterDbContext, ScooterDbContext>();
builder.Services.AddScoped<IEntityService<Scooter>, EntityService<Scooter>>();
builder.Services.AddScoped<IScooterService, ScooterService>();
builder.Services.AddScoped<IRentalCompanyService, RentalCompanyService>();
builder.Services.AddScoped<IDbService, DbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
