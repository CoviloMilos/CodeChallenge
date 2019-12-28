using System;
using System.Threading;
using System.Threading.Tasks;
using CodeChallenge.Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DistributedExceptionHandler.Services
{
    public class ConsumeRabbitMqBackgroundService : BackgroundService
    {
        private readonly ILoggerManager _logger;
        private IServiceScopeFactory _serviceScopeFactory;
        public ConsumeRabbitMqBackgroundService(ILoggerManager logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
           _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();  
            
            using(var scope = _serviceScopeFactory.CreateScope()) 
            {
                var rabbit = (IRabbitConsume) scope.ServiceProvider.GetRequiredService(typeof(IRabbitConsume));
                rabbit.Consume();
            }
            
            return Task.CompletedTask; 
        }
        
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)  {  }  
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) {  }  
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) {  }  
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) {  }  
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)  {  }  
    
        public override void Dispose()  
        {  
            base.Dispose();  
        } 
    }
}