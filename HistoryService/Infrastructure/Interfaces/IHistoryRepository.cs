using HistoryService.Domain;

namespace HistoryService.Infrastructure.Interfaces;

public interface IHistoryRepository
{
    Task<IEnumerable<OperationEntry>> GetOperationHistory();
    
    Task<OperationEntry> CreateOperationEntry(OperationEntry entry);
}