using System.Threading.Tasks;
using DistributedExceptionHandler.Models;

namespace DistributedExceptionHandler.Interfaces
{
    public interface IExceptionRepository
    {
         Task AddException(ExceptionModel exceptionModel);
         Task<bool> SaveAll();
    }
}