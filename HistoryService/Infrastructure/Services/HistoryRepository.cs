using HistoryService.Domain;
using HistoryService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Infrastructure.Services;

public class HistoryRepository : IHistoryRepository
{
    private readonly HistoryContext _dbContext;

    public HistoryRepository(HistoryContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<OperationEntry>> GetOperationHistory()
    {
        return await _dbContext.OperationEntries
            .OrderByDescending(o => o.TimeStamp)
            .ToListAsync(); 
    }

    public async Task<OperationEntry> CreateOperationEntry(OperationEntry operationEntry)
    {
        _dbContext.OperationEntries.Add(operationEntry);
        await _dbContext.SaveChangesAsync();
        return operationEntry;
    }
}