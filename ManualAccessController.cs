using Application.CQRS.Core;
using Application.CQRS.ManualAccess.Commands;
using Application.CQRS.ManualAccess.Queries;
using Application.Extension;
using Domain.DTOs.Base;
using Domain.DTOs.ManualAccess;
using Domain.Models.ManualAccess;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/manual-access")]
	[ApiController]
	[ApiVersion("1.0")]
	public class ManualAccessController : BaseController
	{
		[HttpPost]
		[Route("create")]
		public async Task<ActionResult<ApiResult<bool>>> Create(CreateManualAccessRequestModel input)
		{
			var command = new CreateManualAccessCommand(input);
			return await Mediator.HandleRequest(command, HttpContext);
		}

		[HttpPost]
		[Route("list")]
		public async Task<ActionResult<ApiResult<PagedResult<ManualAccessDto>>>> GetAll(GetManualAccessRequestModel input)
		{
			var query = new GetManualAccessQuery(input);
			return await Mediator.HandleRequest(query, HttpContext);
		}

		[HttpGet]
		[Route("get/{id}")]
		public async Task<ActionResult<ApiResult<ManualAccessByIdDto>>> Get(int id)
		{
			var query = new GetManualAccessByIdQuery(id);
			return await Mediator.HandleRequest(query, HttpContext);
		}

		[HttpDelete]
		[Route("{id}")]
		public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
		{
			var command = new DeleteManualAccessCommand(id);
			return await Mediator.HandleRequest(command, HttpContext);
		}
	}
}
