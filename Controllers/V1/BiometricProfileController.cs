using Application.CQRS.BiometricProfile.Commands;
using Application.CQRS.BiometricProfile.Queries;
using Application.CQRS.BiometricProfileHistory.Queries;
using Application.CQRS.Core;
using Application.Extension;
using Domain.DTOs.Base;
using Domain.DTOs.BiometricProfile;
using Domain.Models.BiometricProfile;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
	[Route("api/v{version:apiVersion}/biometric-profile")]
	[ApiController]
	[ApiVersion("1.0")]
	public class BiometricProfileController : BaseController
	{
		[HttpPost]
		[Route("list")]
		public async Task<ActionResult<ApiResult<PagedResult<BiometricProfileDto>>>> GetAll(GetBiometricProfileRequestModel input)
		{
			var query = new GetBiometricProfileQuery(input);
			return await Mediator.HandleRequest(query, HttpContext);
		}

		[HttpPost]
		[Route("history")]
		public async Task<ActionResult<ApiResult<List<BiometricProfileHistoryDto>>>> GetHistory(GetBiometricProfileHistoryRequestModel input)
		{
			var query = new GetBiometricProfileHistoryQuery(input);
			return await Mediator.HandleRequest(query, HttpContext);
		}

		[HttpPut]
		[Route("status")]
		public async Task<ActionResult<ApiResult<bool>>> Updatetatus(UpdateBiometricProfileStatusRequestModel input)
		{
			var query = new UpdateBiometricProfileStatusCommand(input);
			return await Mediator.HandleRequest(query, HttpContext);
		}

		[HttpGet]
        [Route("document/types")]
        public async Task<ActionResult<ApiResult<List<DocumentTypeDto>>>> GetDocumentTypes()
        {
            var query = new GetBiometricProfilesDocumentTypesQuery();
            return await Mediator.HandleRequest(query, HttpContext);
        }
    }
}
