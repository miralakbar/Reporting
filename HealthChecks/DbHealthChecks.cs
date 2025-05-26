using Application.Repositories.Base;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.HealthChecks
{
    public class DbHealthChecks : IHealthCheck
	{
		private readonly IUnitOfWork _unitOfWork;

		public DbHealthChecks(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
		{
			try
			{
				int partner = await _unitOfWork.PartnerRepository().CountAsync();

				if (partner >= 1)
				{
					return HealthCheckResult.Healthy("A database operation has been completed successfully. The system has a healthy connection to the database. ");
				}
				return HealthCheckResult.Unhealthy("The system does not have a healthy connection to the database.");

			}
			catch (Exception)
			{
				return HealthCheckResult.Unhealthy("The system does not have a healthy connection to the database.");
			}
		}
	}
}
