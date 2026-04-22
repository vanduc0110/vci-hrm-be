using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TTDesign.API.Controllers;

[Route( "api/[controller]" )]
[ApiController]
[AllowAnonymous]
[Consumes( "application/json" )]
[Produces( "application/json" )]
public class HealthController : ControllerBase
{
  private readonly ILogger<HealthController> _logger;

  public HealthController(ILogger<HealthController> logger)
  {
    _logger = logger;
  }
  
  [HttpGet]
  public Task<IActionResult> CheckHeath()
  {
    _logger.LogInformation( "Check health OK!" );
    return Task.FromResult<IActionResult>( Ok() );
  }
}