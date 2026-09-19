using LendingPlatform.API.Models;

namespace LendingPlatform.API.Services;

public interface ILendingService
{
  ApplicationResponse EvaluateApplication(ApplicationRequest request);
  MetricsResponse GetMetrics();
}