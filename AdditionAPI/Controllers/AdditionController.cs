using AdditionAPI.services;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using Serilog;
using Status = OpenTelemetry.Trace.Status;

namespace AdditionAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AdditionController : ControllerBase
{
    private readonly Addition _additionService;

    public AdditionController(Addition additionService)
    {
        _additionService = additionService;
    }

    [HttpGet]
    public IActionResult Add(int number1, int number2)
    {
        
        using (var activity = TelemetryActivitySource.Instance.StartActivity("API:Add"))
        {
            try
            {
                activity?.SetTag("input.number1", number1);
                activity?.SetTag("input.number2", number2);

                int result =  _additionService.Add(number1, number2);

                
                Log.Information("Addition performed: {Number1} + {Number2} = {Result}", number1, number2, result);

                activity?.SetTag("output.result", result);

                return Ok(new { Number1 = number1, Number2 = number2, Result = result });
            }
            catch (Exception ex)
            {
               
                Log.Error(ex, "An error occurred while processing the addition request.");
                
                activity?.SetStatus(Status.Error.WithDescription(ex.Message));
                activity?.RecordException(ex);

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}