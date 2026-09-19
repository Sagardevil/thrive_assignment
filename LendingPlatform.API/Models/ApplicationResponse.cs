namespace LendingPlatform.API.Models;

public class ApplicationResponse
{
  public bool IsSuccessful { get; set; }
  public string DecisionReason { get; set; } = string.Empty;
  public decimal Ltv { get; set; }
}