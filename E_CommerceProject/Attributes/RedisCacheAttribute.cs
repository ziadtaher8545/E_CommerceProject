using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_CommerceProject.Attributes;

public class RedisCacheAttribute : ActionFilterAttribute
{
    private readonly int durationInSeconds;

    public RedisCacheAttribute(int durationInSeconds=60)
    {
        this.durationInSeconds = durationInSeconds;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();


        var cacheKey = CreateCacheKey(context.HttpContext.Request);
        var data = await cacheService!.GetAsync(cacheKey);
        if (!string.IsNullOrEmpty(data))
        {
            context.Result = new ContentResult()
            {
                Content = data,
                ContentType = "application/json",
                StatusCode = StatusCodes.Status200OK

            };

            return;
        }
        var executedContext = await next.Invoke();
        if (executedContext.Result is OkObjectResult { Value: not null } ok)
        {
            await cacheService.SetAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(durationInSeconds));
        }

    }


    private static string CreateCacheKey(HttpRequest request)
    {

        var cacheKey = new StringBuilder();
        cacheKey.Append(request.Path);
        if (request.Query.Any())
        {
            cacheKey.Append("?");
            foreach (var (k, v) in request.Query.OrderBy(q => q.Key))
            {
                cacheKey.Append(k).Append("=").Append(v).Append("&");

            }


        }
        return cacheKey.ToString();
    }
}
