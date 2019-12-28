using System;
using CodeChallenge.Common.RabbitMq;
using DistributedExceptionHandler.Interfaces;
using DistributedExceptionHandler.Models;
using DistributedExceptionHandler.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DistributedExceptionHandler.Services
{
    public class ChapterLister : RabbitListener
    {
        private readonly IServiceScopeFactory _services;
        public ChapterLister(IServiceScopeFactory services, IOptions<RabbitQueueOptions> rabbitQueueOptions, 
                             IOptions<RabbitOptions> rabbitOptions) 
            : base(rabbitQueueOptions, rabbitOptions)
        {
            _services = services;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool ProcessMessage(string exceptionModelString)
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

        public override string ToString()
        {
            return base.ToString();
        }
    }
}