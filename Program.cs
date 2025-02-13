using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using StudentFunctions.Models.School;

var builder = FunctionsApplication.CreateBuilder(args);

// Load environment variables and configuration
var configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

string connectionString = configuration["DATABASE_CONNECTION_STRING"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("DATABASE_CONNECTION_STRING is not set in the environment variables.");
}

// Add the DbContext and configure the connection string
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(connectionString));

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
