using Microsoft.EntityFrameworkCore;
using Nexus.Shared.Models;


namespace Nexus.Backend;

public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options)
    {

    }
    // TODO: why I need this ?
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=nexus.db");
    public DbSet<Note> Notes { get; set; }
    public DbSet<SavedPassword> SavedPasswords { get; set; }
}
