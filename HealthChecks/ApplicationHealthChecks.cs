using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Application.HealthChecks
{
	public class ApplicationHealthChecks : IHealthCheck
	{
		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			return Task.FromResult(HealthCheckResult.Healthy("The System is up and running."));
		}
	}
}
