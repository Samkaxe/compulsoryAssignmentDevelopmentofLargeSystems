using HistoryService.Application.Interfaces;
using HistoryService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HistoryService.API.Controllers;

[ApiController] // Indicates this is an API controller
[Route("api/[controller]")] // Base route will be like 'api/History'
public class HistoryController : ControllerBase
{
    private readonly IHistoryService _historyService;
    

    public HistoryController(IHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet("history")] 
    public async Task<ActionResult<IEnumerable<OperationEntry>>> GetHistory()
    {
        var historyEntries = await _historyService.GetHistory();
        return Ok(historyEntries);
    }
}