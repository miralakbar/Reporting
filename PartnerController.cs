using Application.CQRS.Core;
using Application.CQRS.Partners.Commands;
using Application.CQRS.Partners.Queries;
using Application.CQRS.Partners.Queries.Application.CQRS.Partners.Queries;
using Application.Extension;
using Domain.DTOs.Base;
using Domain.DTOs.Partner;
using Domain.Models.Partner;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/partner")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PartnerController : BaseController
    {
        [HttpPost]
        [Route("list")]
        public async Task<ActionResult<ApiResult<PagedResult<PartnerDto>>>> PartnerList(PartnerRequestModel input)
        {
            var query = new GetPartnerListQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<ApiResult<PartnerDto>>> AddPartner(AddPartnerRequestModel input)
        {
            var command = new AddPartnerCommand(input);
            return await Mediator.HandleRequest(command, HttpContext);
        }

        #region PARENT PARTNER API SERVICES

        [HttpPost]
        [Route("parent-partner/list")]
        public async Task<ActionResult<ApiResult<PagedResult<ParentPartnerDto>>>> ParentPartnerList(ParentPartnerRequestModel input)
        {
            var query = new GetParentPartnerListQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPost]
        [Route("parent-partner/add")]
        public async Task<ActionResult<ApiResult<ParentPartnerDto>>> AddParentPartner(AddParentPartnerRequestModel input)
        {
            var command = new AddParentPartnerCommand(input);
            return await Mediator.HandleRequest(command, HttpContext);
        }

        [HttpGet]
        [Route("parent-partner/{id}")]
        public async Task<ActionResult<ApiResult<ParentPartnerDto>>> GetParentPartnerById(int id)
        {
            var query = new GetParentPartnerByIdQuery(id);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPut]
        [Route("parent-partner")]
        public async Task<ActionResult<ApiResult<bool>>> UpdateParentPartner(UpdateParentPartnerRequestModel input)
        {
            var query = new UpdateParentPartnerCommand(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpDelete]
        [Route("parent-partner")]
        public async Task<ActionResult<ApiResult<bool>>> DeleteParentPartner(int id)
        {
            var query = new DeleteParentPartnerQuery(id);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        #endregion
    }
}
