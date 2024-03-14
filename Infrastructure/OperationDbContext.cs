using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class OperationDbContext : DbContext
{
    
    
    public DbSet<Operation> Operations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = @"..\Infrastructure\operation.db";
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        // For SQL Server (as a comment mentioned), you typically wouldn't adjust the path since it's not file-based.
        // optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=OperationDb;Trusted_Connection=True;");
    }
}