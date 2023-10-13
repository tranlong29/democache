using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SiUtils;
using System.Text;

namespace Utils.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        public CacheAttribute() { }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cache = CacheDuLieu.Instance;
            var cachekey = GenCacheKey(context.HttpContext.Request);
            try
            {
                var cacheRepo = cache.GetString(cachekey);
                if (!string.IsNullOrEmpty(cacheRepo))
                {
                    var contentaResult = new ContentResult { Content = cacheRepo, ContentType = "application/json", StatusCode = 200 };
                    context.Result = contentaResult;
                    return;
                }
                var excutedContext = await next();
                if (excutedContext.Result is OkObjectResult okObjectResult)
                {
                    var result = JsonConvert.SerializeObject(okObjectResult.Value);
                    cache.SetString(cachekey, result);
                }
            }
            catch (Exception ex) { }

        }

        private static string GenCacheKey(HttpRequest request)
        {
            var keyBuiler = new StringBuilder();
            keyBuiler.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuiler.Append($"{key} : {value}");

            }
            return keyBuiler.ToString();
        }
        

    }
}
