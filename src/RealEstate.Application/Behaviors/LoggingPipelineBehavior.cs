using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RealEstate.Domain;

namespace RealEstate.Application.Behaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Starting request {@RequestName}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                DateTime.UtcNow);


            try
            { 
                var result = await next();

                _logger.LogInformation(
                    "Completed request {@RequestName}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    DateTime.UtcNow);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    //ex, // Passing the exception object automatically logs the StackTrace
                    "Request {@RequestName} failed with exception: {@ErrorMessage}, {@DateTimeUtc}",
                    typeof(TRequest).Name,
                    ex.Message,
                    DateTime.UtcNow);

                // CRITICAL: Re-throw the exception so your GlobalExceptionHandler can catch it and format the API response!
                throw;
            }

           
        }
    }
}