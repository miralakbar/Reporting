using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace API.Swagger
{
    public class HmacAuthenticationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Identifier",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Required = true
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Signature",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Required = true
            });

			operation.Parameters.Add(new OpenApiParameter
			{
				Name = "Auth-Scheme",
				In = ParameterLocation.Header,
				Schema = new OpenApiSchema
				{
					Type = "string"
				},
				Required = true
			});
		}
    }
}
