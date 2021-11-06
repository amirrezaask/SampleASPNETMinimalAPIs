using Microsoft.EntityFrameworkCore;
using Nexus.Web.Handlers;

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

// Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

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
app.Run();



public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options)
    {

    }
    // TODO: why I need this ?
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=nexus.db");
    public DbSet<Note?> Notes { get; set; }
}

public class Note
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}