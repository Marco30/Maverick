
using System.Reflection;
using Courses.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using WarGamesAPI.Data;
using WarGamesAPI.Helpers;
using WarGamesAPI.Interfaces;
using WarGamesAPI.Services;
using WarGamesAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["WarGamesConfiguration:Uri"]!))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{configuration["ApplicationName"]}-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
        NumberOfShards = 2,
        NumberOfReplicas = 1
    })
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName ?? "unknown")
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .CreateLogger();


// Add services to the container.
builder.Services.AddDbContext<WarGamesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Configuration.AddEnvironmentVariables().AddUserSecrets(Assembly.GetExecutingAssembly(), true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name:"AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WarGames API", Version = "v1" });

  
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);



builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IValidationRepository, ValidationRepository>();
builder.Services.AddScoped<IGptService, GptService>();


builder.Logging.ClearProviders(); // Remove default loggers
builder.Logging.AddSerilog(); // Add Serilog logger

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
