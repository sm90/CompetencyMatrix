using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CompetencyMatrix.Infrastructure
{
    public static class GlobalExceptionHandlerCreator
    {
        public const string ERROR_MESSAGE = "An unhandled exception has occurred while executing the request";

        public static ExceptionHandlerOptions CreateGlobalExceptionHandler(ILoggerFactory loggerFactory)
        {
            return new ExceptionHandlerOptions
            {
                ExceptionHandler = context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(LoggingEvents.GlobalUnhandledException, exception, exception.Message);

                    return Task.FromResult(0);
                },
                ExceptionHandlingPath = null
            };
        }
    }
}