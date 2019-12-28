using System.Threading.Tasks;
using DistributedExceptionHandler.Interfaces;
using DistributedExceptionHandler.Models;

namespace DistributedExceptionHandler.Repositories
{
    public class ExceptionRepository : IExceptionRepository
    {
        private ExceptionDataContext _context;

        public ExceptionRepository(ExceptionDataContext context) 
        {
            _context = context;
        }

        public void AddException(ExceptionModel exceptionModel)
        {
            _context.AddAsync(exceptionModel);
        }

        public bool SaveAll()
        {
            var a = true;
            try
            {
              a=  _context.SaveChanges() > 0;   
            }
            catch (System.Exception)
            {
                throw;
            }
            return a;
        }
    }
}