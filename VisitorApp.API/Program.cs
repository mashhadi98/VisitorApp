using VisitorApp.API.Common.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure all services using dedicated service extensions
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddMappingServices();
builder.Services.AddCorsServices();
builder.Services.AddLoggingServices();
builder.Services.AddDependencyInjectionServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiDocumentationServices();

var app = builder.Build();

// Apply database migrations
//await app.ApplyDatabaseMigrationsAsync();
await app.AddDatabaseApplication(builder.Configuration);

// Configure the HTTP request pipeline
app.ConfigurePipeline();

app.Run();


