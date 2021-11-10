using Microsoft.EntityFrameworkCore;
using Nexus.Shared.Models;


namespace Nexus.Backend;

public class NexusDbContext : DbContext
{
    public NexusDbContext(DbContextOptions<NexusDbContext> options) : base(options)
    {

    }
    
    public DbSet<Note> Notes { get; set; }
    public DbSet<SavedPassword> SavedPasswords { get; set; }
}
