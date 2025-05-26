using Application.CQRS.Core;
using Application.CQRS.Customer.Queries;
using Application.Extension;
using Domain.DTOs.Base;
using Domain.DTOs.Customer;
using Domain.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/customer")]
    [ApiController]
    [ApiVersion("1.0")]
    //[ApiExplorerSettings(IgnoreApi = true)] //ignore
    public class CustomerController : BaseController
    {
        [HttpPost]
        [Route("customers")]
        public async Task<ActionResult<ApiResult<PagedResult<CustomersDto>>>> Getcustomers(CustomerRequestModel input)
        {
            var query = new GetCustomersQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpGet]
        [Route("customers/{id}")]
        public async Task<ActionResult<ApiResult<CustomerDto>>> GetCustomerById(Guid id)
        {
            var query = new GetCustomerByIdQuery(id);
            return await Mediator.HandleRequest(query, HttpContext);
        }
    }
}
