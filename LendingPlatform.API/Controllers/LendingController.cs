using LendingPlatform.API.Models;
using LendingPlatform.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LendingPlatform.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LendingController : ControllerBase
{
  private readonly ILendingService _lendingService;

  public LendingController(ILendingService lendingService)
  {
    _lendingService = lendingService;
  }

  [HttpPost("apply")]
  public IActionResult Evaluate([FromBody] ApplicationRequest request)
  {
    if (request == null)
    {
      return BadRequest("Invalid request payload.");
    }

    var result = _lendingService.EvaluateApplication(request);
    return Ok(result);
  }

  [HttpGet("metrics")]
  public IActionResult GetMetrics()
  {
    var metrics = _lendingService.GetMetrics();
    return Ok(metrics);
  }
}