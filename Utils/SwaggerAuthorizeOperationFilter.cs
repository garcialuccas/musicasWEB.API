using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace musicasWEB.API.Utils
{
    public class SwaggerAuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var method = context.MethodInfo;

            var anonymos = method.IsDefined(typeof(AllowAnonymousAttribute), true)
                || (method.DeclaringType?.IsDefined(typeof(AllowAnonymousAttribute), true) ?? false);

            var authorized = method.IsDefined(typeof(AuthorizeAttribute), true)
                || (method.DeclaringType?.IsDefined(typeof(AuthorizeAttribute), true) ?? false);

            if (anonymos || !authorized) return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>()
            });
        }
    }
}
