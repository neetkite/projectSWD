using BeanlancerAPI2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanlancerAPI2.Cache
{
  //  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSecond;

        public CachedAttribute(int timeToLiveSecond)
        {
            _timeToLiveSecond = timeToLiveSecond;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //check neu request dc cache chua
            // neu roi  thi return
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);


            if (string.IsNullOrEmpty(cacheResponse))
            {

                var contenResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = 200
                };
                context.Result = contenResult;
                return; 
            }
            var executedContext = await next();
            if(executedContext.Result is OkObjectResult okResut)
            {
                await cacheService.CacheResponseAsync(cacheKey, okResut.Value, TimeSpan.FromSeconds(_timeToLiveSecond));
            }
          
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");
            foreach(var (key,value) in request.Query.OrderBy(x => x.Key)){
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
