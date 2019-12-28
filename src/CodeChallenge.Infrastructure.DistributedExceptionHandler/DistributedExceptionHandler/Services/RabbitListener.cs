using System.Diagnostics;
using System.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
using DistributedExceptionHandler.Interfaces;
using DistributedExceptionHandler.RabbitMq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using DistributedExceptionHandler.Models;
using CodeChallenge.Common.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using DistributedExceptionHandler.Repositories;
using CodeChallenge.Common.RabbitMq;

namespace DistributedExceptionHandler.Services
{
    public class RabbitListener : IHostedService
    { 
        private readonly RabbitQueueOptions _rabbitQueueOptions;
        private readonly RabbitOptions _rabbitOptions;
        private readonly IConnection connection;
        private readonly IModel channel;
  
        public RabbitListener(IOptions<RabbitQueueOptions> rabbitQueueOptions,
                              IOptions<RabbitOptions> rabbitOptions)
        {
            _rabbitQueueOptions = rabbitQueueOptions.Value;
            _rabbitOptions = rabbitOptions.Value;
            
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitOptions.HostName,
                    UserName = _rabbitOptions.UserName,
                    Password = _rabbitOptions.Password,
                    VirtualHost = _rabbitOptions.VHost,
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitListener init error,ex:{ex.Message}");
            }
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            RegisterQueue();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            UnRegisterConnection();
            return Task.CompletedTask;
        }

        public void RegisterQueue()
        {
            var consumer = new EventingBasicConsumer(this.channel);
            consumer.Received += (model, ea) => 
            {
                var body = Encoding.UTF8.GetString(ea.Body);

                if(ProcessMessage(body))
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: _rabbitQueueOptions.Queue, consumer: consumer);
        }

        public void UnRegisterConnection()
        {
            this.connection.Close();
        }

        public virtual bool ProcessMessage(String exceptionModelString)
        {
            throw new NotImplementedException();
        }
        /*public bool ProcessMessage(String exceptionModelString)
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
        }*/
    }
}