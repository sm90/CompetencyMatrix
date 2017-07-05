using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace CompetencyMatrix.Infrastructure
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            String message = String.Empty;
            HttpResponse response = context.HttpContext.Response;
            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(TimeExpiredException))
            {
                message = context.Exception.ToString();
                status = HttpStatusCode.RequestTimeout;
                response.HttpContext.Response.Redirect("~/Account/Login");
                response.StatusCode = (int)status;
            }
            else
            {
                message = context.Exception.Message;
                status = HttpStatusCode.NotFound;
            }
            //HttpResponse response = context.HttpContext.Response;
            
            //response.StatusCode = (int)status;
            //response.ContentType = "application/json";
            //var err = message + " " + context.Exception.StackTrace;
            //response.WriteAsync(err);
        }
    }
}
