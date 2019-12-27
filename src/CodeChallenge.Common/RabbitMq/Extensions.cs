using System.Collections.Specialized;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace CodeChallenge.Common.RabbitMq
{
    public static class Extensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("rabbitmq-connection");
            services.Configure<RabbitOptions>(section);
            
            var sectionExchange = configuration.GetSection("rabbitmq-configuration");
            services.Configure<RabbitExchangeOptions>(sectionExchange);

            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>(); 
            services.AddSingleton<IRabbitManager, RabbitManager>();
  
            return services;
        }
    }
}