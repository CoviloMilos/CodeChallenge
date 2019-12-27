using CodeChallenge.Common.RabbitMq;
using DistributedExceptionHandler.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace DistributedExceptionHandler.RabbitMq
{
    public static class Extensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            
            var section = configuration.GetSection("rabbitmq-connection");
            services.Configure<RabbitOptions>(section);

            var sectionQueue = configuration.GetSection("rabbitmq-configuration");
            services.Configure<RabbitQueueOptions>(sectionQueue);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>(); 
            services.AddScoped<IRabbitConsume, RabbitConsume>();
  
            return services;
        }
    }
}