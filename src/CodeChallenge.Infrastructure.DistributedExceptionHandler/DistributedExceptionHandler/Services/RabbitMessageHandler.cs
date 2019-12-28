using System;
using CodeChallenge.Common.Logging;
using DistributedExceptionHandler.Interfaces;
using DistributedExceptionHandler.Models;
using DistributedExceptionHandler.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace DistributedExceptionHandler.Services
{
    public class RabbitMessageHandler : RabbitBackgroundWorker
    {
        private readonly IServiceScopeFactory _services;
        private readonly ILoggerManager _logger;

        public RabbitMessageHandler(IServiceScopeFactory services,
                                    IPooledObjectPolicy<IModel> objectPolicy, 
                                    IOptions<RabbitQueueOptions> rabbitQueueOptions,
                                    ILoggerManager logger) 
            : base(objectPolicy, rabbitQueueOptions, logger)
        {
            _services = services;
            _logger = logger;
        }


        public override bool ProcessRabbitMqMessage(string exceptionModelString)
        {
            try
            {
                var exceptionModel = ExceptionModel.DeserializeModel(exceptionModelString);

                using(var scope = _services.CreateScope())
                {
                    var exceptionRepositoryService = (IExceptionRepository) scope
                            .ServiceProvider
                            .GetRequiredService(typeof(IExceptionRepository));

                    _logger.LogInfo($"Inserting exception message {exceptionModel.ExceptionGuid} to postgres database");
                    exceptionRepositoryService.AddException(exceptionModel);

                    if (base.getFirstStartFlag())
                    {
                        _logger.LogInfo("Restoring all Exception Model data from database for SignalR. This is first start of application.");
                    }
                    
                    base.setFirstStartFlag(false);
                    return exceptionRepositoryService.SaveAll();
                }
            }
            catch (JsonReaderException jsonReaderException)
            {
                _logger.LogError("Could not Deserialize Exception Model. Exception: " + jsonReaderException.Message);
                return false;
            }
            catch (Exception exception)
            {
                _logger.LogError("Could not create Services Scoper or insert Exception Model in database. Exception: " + exception.Message);
                return false;
            }
        }
    }
}