using Azure;
using CarRental.ModelforController;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Repository
{
    public class ExceptionHandler : IMiddleware
    {
        private readonly ILogger _logger;
        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message, ex);
                await OnException(ex,context);
       

            }
            
        }
       
        public Task OnException(Exception ex,HttpContext context)
        {
            CustomException cs = new CustomException()
            {
                statuscode=500,
                message=ex.Message
                
            };
            cs.ToString();
            return context.Response.WriteAsJsonAsync(cs);
            

        }
    }
}
