using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project.Services.Services
{
    public class BaseService
    {
        private readonly DbContext _context;

        public BaseService(DbContext context)
        {
            this._context = context;
        }

        protected async Task<bool> SaveChangesAsync()
        {
            int rolesAltered = await this._context.SaveChangesAsync();

            return rolesAltered > 0;
        }
    }
}
