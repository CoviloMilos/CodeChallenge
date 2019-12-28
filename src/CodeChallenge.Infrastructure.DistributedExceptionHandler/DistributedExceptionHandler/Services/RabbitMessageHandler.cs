using System;
using DistributedExceptionHandler.Interfaces;
using DistributedExceptionHandler.Models;
using DistributedExceptionHandler.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DistributedExceptionHandler.Services
{
    public class RabbitMessageHandler : RabbitBackgroundWorker
    {
        private readonly IServiceScopeFactory _services;

        public RabbitMessageHandler(IServiceScopeFactory services,
                                    IPooledObjectPolicy<IModel> objectPolicy, 
                                    IOptions<RabbitQueueOptions> rabbitQueueOptions) 
            : base(objectPolicy, rabbitQueueOptions)
        {
            _services = services;
        }

        public override bool ProcessRabbitMqMessage(string exceptionModelString)
        {
            try
            {
                var exceptionModel = ExceptionModel.DeserializeModel(exceptionModelString);

                using(var scope = _services.CreateScope())
                {
                    var exceptionRepositoryService = (IExceptionRepository) scope.ServiceProvider.GetRequiredService(typeof(IExceptionRepository));
                    exceptionRepositoryService.AddException(exceptionModel);
                    return exceptionRepositoryService.SaveAll();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("RabbitMq with that and that Code missed");
                return false;
                //throw new CustomException("rabbitmq_message_deserialization_failed", $"Deserialization of RabbitMq message body to ExceptionModel failed."); 
            }
        }


    }
}