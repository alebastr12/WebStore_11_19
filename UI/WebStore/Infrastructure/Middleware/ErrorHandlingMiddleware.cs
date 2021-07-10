using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorHandlingMiddleware> _Logger;

        public ErrorHandlingMiddleware(RequestDelegate Next, ILogger<ErrorHandlingMiddleware> Logger)
        {
            _Next = Next;
            _Logger = Logger;
        }

        public async Task Invoke(HttpContext Context)
        {
            try
            {
                await _Next(Context);
            }
            catch (Exception Error)
            {
                await HandleExceptionAsync(Context, Error);
                //throw Error;
                throw;
            }
        }

        private Task HandleExceptionAsync(HttpContext Context, Exception Error)
        {
            _Logger.LogError(Error, "Ошибка при обработке запроса {0}", Context.Request.Path);
            return Task.CompletedTask;
        }
    }
}
