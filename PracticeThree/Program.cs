using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.File;
using UPB.CoreLogic.Managers;

var loggerConfiguration = new LoggerConfiguration();
if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.WriteTo.File("logs/PracticeThree.log");
}
else
{
    loggerConfiguration.WriteTo.File("logs/PracticeThree.log");
}

Log.Logger = loggerConfiguration.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<PatientManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

IConfiguration Configuration = configurationBuilder.Build();
string siteTitle = Configuration.GetSection("Title").Value;

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = siteTitle
    });
});

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.EnvironmentName == "Development" || app.Environment.EnvironmentName == "QA" || app.Environment.EnvironmentName == "UAT")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Log.Information("Application raised in " + app.Environment.EnvironmentName + " environment.");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
