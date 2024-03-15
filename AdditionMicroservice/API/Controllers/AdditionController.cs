using AdditionMicroservice.Application.DTOs;
using AdditionMicroservice.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdditionMicroservice.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AdditionController : ControllerBase 
{
    private readonly IAdditionService _additionService;

    public AdditionController(IAdditionService additionService)
    {
        _additionService = additionService;
    } 
    
    [HttpPost]
    public async Task<ActionResult<decimal>> Calculate([FromBody] AdditionOperationDto dto)
    {
        try
        {
            var result = _additionService.Add(dto);
            return Ok(result); // Return 200 OK with the result
        }
        catch (Exception ex)
        {
            // Consider logging the exception:
            // _logger.LogError(ex, "Error during addition calculation");
            return BadRequest(ex.Message); // Return 400 Bad Request with error message
        }
    }
}