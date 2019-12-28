using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeChallenge.Common.Logging;
using DistributedExceptionHandler.RabbitMq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DistributedExceptionHandler.Services
{
    public class RabbitBackgroundWorker : IHostedService
    {

        private readonly DefaultObjectPool<IModel> _objectPool;  
        private readonly RabbitQueueOptions _rabbitQueueOptions;
        private readonly ILoggerManager _logger;
        private bool firstStartOfWorkerFlag = false;
  
        public RabbitBackgroundWorker(IPooledObjectPolicy<IModel> objectPolicy,
                                      IOptions<RabbitQueueOptions> rabbitQueueOptions,
                                      ILoggerManager logger)  
        {  
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);  
            _rabbitQueueOptions = rabbitQueueOptions.Value;
            _logger = logger;
        }  

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInfo("RabbitBackgroundWorker started");
            setFirstStartFlag(true);
            ConsumeMessage();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void ConsumeMessage()
        {
            var channel = _objectPool.Get();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>  
            {     
                var body = Encoding.UTF8.GetString(ea.Body);
                _logger.LogInfo("Consuming messages");
                if(ProcessRabbitMqMessage(body))
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };  
            channel.BasicConsume(_rabbitQueueOptions.Queue, false, consumer);  
        }

        public virtual bool ProcessRabbitMqMessage(String exceptionModelString) 
        {
            throw new NotImplementedException();
        }

        public void setFirstStartFlag(bool firstStartOfWorkerFlag) 
        {
            this.firstStartOfWorkerFlag = firstStartOfWorkerFlag;
        }

        public bool getFirstStartFlag() 
        {
            return this.firstStartOfWorkerFlag;
        }

    }
}