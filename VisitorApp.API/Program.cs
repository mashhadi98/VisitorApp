using VisitorApp.API.Common.Configuration;

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

// Add Identity and OpenIddict services
builder.Services.AddIdentityServices();
builder.Services.AddOpenIddictServices(builder.Configuration);

// Add Authentication and Authorization
builder.Services.AddAuthentication()
    .AddCookie()
    .AddBearerToken();
    
builder.Services.AddAuthorization();

var app = builder.Build();

// Apply database migrations
//await app.ApplyDatabaseMigrationsAsync();
await app.AddDatabaseApplication(builder.Configuration);

// Seed OpenIddict data
await app.SeedOpenIddictDataAsync();

// Configure the HTTP request pipeline
app.ConfigurePipeline();

app.Run();


