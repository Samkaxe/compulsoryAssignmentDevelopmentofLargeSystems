using HistoryService.Application.Interfaces;
using HistoryService.Domain;
using HistoryService.Infrastructure.Interfaces;

namespace HistoryService.Application.Services;

public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;

    public HistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task<IEnumerable<OperationEntry>> GetHistory()
    {
        return await _historyRepository.GetOperationHistory();
    }

    public async Task<OperationEntry> AddEntry(OperationEntry entry)
    {
        return await _historyRepository.CreateOperationEntry(entry);
    }
}