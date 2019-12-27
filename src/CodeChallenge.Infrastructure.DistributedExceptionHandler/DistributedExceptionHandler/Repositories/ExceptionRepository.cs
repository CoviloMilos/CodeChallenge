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

        public async Task AddException(ExceptionModel exceptionModel)
        {
            await _context.AddAsync(exceptionModel);
        }

        public async Task<bool> SaveAll()
        {
            var a = true;
            try
            {
              a= await _context.SaveChangesAsync() > 0;   
            }
            catch (System.Exception)
            {
                throw;
            }
            return a;
        }
    }
}