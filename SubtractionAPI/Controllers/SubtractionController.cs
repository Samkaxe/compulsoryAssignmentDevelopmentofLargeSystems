using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using Serilog;
using SubtractionAPI.services;
using Status = OpenTelemetry.Trace.Status;

namespace SubtractionAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class SubtractionController  : ControllerBase
{
    private readonly Subtraction _subtractionService;

    public SubtractionController(Subtraction subtractionService)
    {
        _subtractionService = subtractionService;
    }
    
    [HttpGet]
    public IActionResult Subtract(int number1, int number2)
    {
        
        using (var activity = TelemetryActivitySource.Instance.StartActivity("API:Subtract"))
        {
            try
            {
                activity?.SetTag("input.number1", number1);
                activity?.SetTag("input.number2", number2);

                int result =  _subtractionService.Subtract(number1, number2);

                
                Log.Information("Subtract performed: {Number1} + {Number2} = {Result}", number1, number2, result);

                activity?.SetTag("output.result", result);

                return Ok(new { Number1 = number1, Number2 = number2, Result = result });
            }
            catch (Exception ex)
            {
               
                Log.Error(ex, "An error occurred while processing the Subtract request.");
                
                activity?.SetStatus(Status.Error.WithDescription(ex.Message));
                activity?.RecordException(ex);

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}