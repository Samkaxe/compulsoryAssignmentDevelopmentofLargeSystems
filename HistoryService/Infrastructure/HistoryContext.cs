using HistoryService.Domain;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Infrastructure;

public class HistoryContext: DbContext
{

    public DbSet<OperationEntry> OperationEntries { get; set; }
    
    public HistoryContext(DbContextOptions<HistoryContext> options) : base(options) { }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source=db.db");
    }
}