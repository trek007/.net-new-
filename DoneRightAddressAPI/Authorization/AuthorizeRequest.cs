using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DoneRightAddressAPI.CustomAttributes
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class AuthorizeRequest : Attribute, IAsyncActionFilter
    {
        private const string apiKeyName = "API_SECRET_KEY";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (!context.HttpContext.Request.Headers.TryGetValue(apiKeyName, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "Api key not found."
                };
                return;
            }
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var key = appSettings.GetValue<string>(apiKeyName);

            if (!key.Equals(extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api key is not valid."
                };
                return;
            }
            await next();
        }
    }
}