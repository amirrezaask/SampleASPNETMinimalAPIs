using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SampleASPNETMinimalAPIs.Backend.Tests;

public class BaseHandlerTest
{
    internal SampleASPNETMinimalAPIsDbContext _dbContext;
    public BaseHandlerTest()
    {
        var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
        keepAliveConnection.Open();
        var options = new DbContextOptionsBuilder<SampleASPNETMinimalAPIsDbContext>().UseSqlite(keepAliveConnection).Options;
        var dbContext = new SampleASPNETMinimalAPIsDbContext(options);
        dbContext.Database.EnsureCreated();
        _dbContext = dbContext;
    }

}