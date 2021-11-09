using Microsoft.EntityFrameworkCore;
using Nexus.Backend;
using Nexus.Backend.Handlers;
var builder = WebApplication.CreateBuilder(args);

// Database
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSqlite<NexusDbContext>("Data Source=nexus.db");
}
else
{
    builder.Services.AddNpgsql<NexusDbContext>(
        builder.Configuration.GetConnectionString("Database"));
}
// Add Cors middleware
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyCORS",
                      builder =>
                      {
                          builder.AllowAnyOrigin();
                      });
});

// Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Build application DI
var app = builder.Build();

// Cors Settings
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nexus Cloud APIs");
        c.RoutePrefix = String.Empty;
    });

}
app.MapNotes("/api/v1");
app.MapSavedPassword("/api/v1");

app.Run();