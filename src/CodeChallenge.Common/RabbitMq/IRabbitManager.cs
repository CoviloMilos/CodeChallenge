using System.Threading.Tasks;

namespace CodeChallenge.Common.RabbitMq
{
    public interface IRabbitManager
    {
        void Publish<T>(T message) where T : class;  
    }
}