using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Nexus.Backend.Tests;

public class BaseHandlerTest
{
    internal NexusDbContext _dbContext;
    public BaseHandlerTest()
    {
        var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
        keepAliveConnection.Open();
        var options = new DbContextOptionsBuilder<NexusDbContext>().UseSqlite(keepAliveConnection).Options;
        var dbContext = new NexusDbContext(options);
        dbContext.Database.EnsureCreated();
        _dbContext = dbContext;
    }
    
}