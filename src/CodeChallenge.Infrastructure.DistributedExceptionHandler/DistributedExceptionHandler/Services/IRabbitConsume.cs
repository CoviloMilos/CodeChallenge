using System.Threading.Tasks;

namespace DistributedExceptionHandler.Services
{
    public interface IRabbitConsume
    {
        void Consume();
    }
}