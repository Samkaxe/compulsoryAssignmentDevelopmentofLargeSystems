using HistoryService.Domain;

namespace HistoryService.Application.Interfaces;

public interface IHistoryService
{
    Task<IEnumerable<OperationEntry>> GetHistory();
    Task<OperationEntry> AddEntry(OperationEntry entry); 
}