using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Prm.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Prm.API.Helpers
{
    public class LogUser : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var scoreContext = await next();
            var userId = int.Parse(scoreContext.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier).Value);
            var prm = scoreContext.HttpContext.RequestServices.GetService<IPrmRepository>();
            var user = await prm.GetUser(userId, true);
            user.LastActive = DateTime.Now;
            await prm.SaveAll();
        }
    }
}