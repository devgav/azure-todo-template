using TodoApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Configure Azure Key Vault access with credentials from environment variables
var keyVaultAccountEndpoint = Environment.GetEnvironmentVariable("ACCOUNT_ENDPOINT");
var keyVaultAccountKey = Environment.GetEnvironmentVariable("ACCOUNT_KEY");
var keyVaultDatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
                      });
});
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

if (!string.IsNullOrEmpty(keyVaultAccountEndpoint) && !string.IsNullOrEmpty(keyVaultAccountKey) && !string.IsNullOrEmpty(keyVaultDatabaseName)) {
    // Add services to the container.
    builder.Services.AddDbContext<TodoContext>(opt =>
        opt.UseCosmos(
            keyVaultAccountEndpoint,
            keyVaultAccountKey,
            keyVaultDatabaseName
        ));
}



var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
