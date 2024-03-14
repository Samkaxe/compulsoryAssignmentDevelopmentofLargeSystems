using HistoryService.Domain;

namespace HistoryService.Infrastructure.Interfaces;

public interface IHistoryRepository
{
    public Task<IEnumerable<OperationEntry>> GetOperationHistory();
    
    public Task<OperationEntry> CreateOperationEntry(OperationEntry entry);
}