using System.Threading.Tasks;
using DistributedExceptionHandler.Models;

namespace DistributedExceptionHandler.Interfaces
{
    public interface IExceptionRepository
    {
         void AddException(ExceptionModel exceptionModel);
         bool SaveAll();
    }
}