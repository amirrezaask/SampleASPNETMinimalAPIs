using Microsoft.EntityFrameworkCore;

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

app.MapGet("/notes", async (NexusDbContext dbContext) => await dbContext.Notes.ToListAsync());
app.MapPost("/notes", async (NexusDbContext dbContext, Note note) =>
{
    note.Id = Guid.NewGuid().ToString();
    await dbContext.Notes.AddAsync(note);
    await dbContext.SaveChangesAsync();
    return note.Id;
});
app.MapGet("/notes/{id:guid}", async (string id, NexusDbContext dbContext) => await dbContext.Notes.FindAsync(id));
app.MapDelete("/notes/{id:guid}", async (string id, NexusDbContext dbContext) =>
{
    dbContext.Remove(new Note {Id = id});
    await dbContext.SaveChangesAsync();
});
app.Run();



class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options)
    {

    }
    // TODO: why I need this ?
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=nexus.db");
    public DbSet<Note> Notes { get; set; }
}

class Note
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}