using Microsoft.EntityFrameworkCore;
using SampleASPNETMinimalAPIs.Shared.Models;


namespace SampleASPNETMinimalAPIs.Backend;

public class SampleASPNETMinimalAPIsDbContext : DbContext
{
    public SampleASPNETMinimalAPIsDbContext(DbContextOptions<SampleASPNETMinimalAPIsDbContext> options) : base(options)
    {

    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<SavedPassword> SavedPasswords { get; set; }
    public DbSet<User> Users { get; set; }
}
