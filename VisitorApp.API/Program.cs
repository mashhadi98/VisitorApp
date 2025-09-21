using VisitorApp.API.Common.Configuration;
using VisitorApp.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure all services using dedicated service extensions
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddMappingServices();
builder.Services.AddCorsServices();
builder.Services.AddLoggingServices(builder.Configuration);
builder.Services.AddDependencyInjectionServices();
builder.Services.AddApplicationServices();
builder.Services.AddApiDocumentationServices();
builder.Services.AddFileStorageServices(builder.Configuration);

// Add Identity and OpenIddict services
builder.Services.AddIdentityServices();
builder.Services.AddOpenIddictServices(builder.Configuration);

// Add JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
    
builder.Services.AddAuthorization();

var app = builder.Build();

// Apply database migrations
//await app.ApplyDatabaseMigrationsAsync();
await app.AddDatabaseApplication(builder.Configuration);

// Seed OpenIddict data
await app.SeedOpenIddictDataAsync();

// Seed default admin user
await VisitorApp.Persistence.Features.Identity.DataSeeder.SeedDataAsync(app.Services);

// Configure the HTTP request pipeline
app.ConfigurePipeline();

// Configure static files (required for serving uploaded files)
app.UseStaticFiles();

// Configure file storage
app.UseFileStorage(builder.Configuration);

// Test file storage service (optional - can be removed in production)
if (app.Environment.IsDevelopment())
{
    await app.TestFileStorageService();
}

app.Run();


