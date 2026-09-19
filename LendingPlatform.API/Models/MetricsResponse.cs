namespace LendingPlatform.API.Models;

public class MetricsResponse
{
  public int TotalApplicants { get; set; }
  public int SuccessfulApplicants { get; set; }
  public int DeclinedApplicants { get; set; }
  public decimal TotalValueWritten { get; set; }
  public decimal MeanLtv { get; set; }
}