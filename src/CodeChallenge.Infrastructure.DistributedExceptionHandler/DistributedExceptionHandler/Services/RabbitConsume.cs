using System;
using System.Threading.Tasks;
using CodeChallenge.Common.Logging;
using DistributedExceptionHandler.Interfaces;
using DistributedExceptionHandler.Models;
using DistributedExceptionHandler.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DistributedExceptionHandler.Services
{
    public class RabbitConsume : IRabbitConsume
    {
        private readonly DefaultObjectPool<IModel> _objectPool;  
        private readonly ILoggerManager _logger;
        private readonly RabbitQueueOptions _rabbitQueueOptions;
        private IExceptionRepository _exceptionRepository;
  
        public RabbitConsume(IPooledObjectPolicy<IModel> objectPolicy, 
                             ILoggerManager logger,
                             IOptions<RabbitQueueOptions> rabbitQueueOptions,
                             IExceptionRepository exceptionRepository)  
        {  
            _logger = logger;
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);  
            _rabbitQueueOptions = rabbitQueueOptions.Value;
            _exceptionRepository = exceptionRepository;
        }  

        public void Consume()
        {
            var channel = _objectPool.Get();

            var consumer = new EventingBasicConsumer(channel);  
            consumer.Received += (ch, ea) =>  
            {     
                HandleMessage(System.Text.Encoding.UTF8.GetString(ea.Body));  
                channel.BasicAck(ea.DeliveryTag, false);  
            };  
    
            consumer.Shutdown += OnConsumerShutdown;  
            consumer.Registered += OnConsumerRegistered;  
            consumer.Unregistered += OnConsumerUnregistered;  
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;  
    
            channel.BasicConsume(_rabbitQueueOptions.Queue, false, consumer);  

        }

        private async void HandleMessage(string content)  
        {  
            var exceptionModel = ExceptionModel.DeserializeModel(content);
            await _exceptionRepository.AddException(exceptionModel);

            if (await _exceptionRepository.SaveAll()) 
            {
                Console.WriteLine($"SAVED CONTENT content: {content}");  
                _logger.LogInfo($"SAVED CONTENT consumer received {content}"); 
            }
             
        }  


        //For some complex implementation
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)  {  }  
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) {  }  
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) {  }  
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) {  }  
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)  {  } 
    }
}