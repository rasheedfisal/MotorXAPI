using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorX.DataService.Data;
using MotorX.DataService.Entities;
using MotorX.DataService.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Repository
{
    public class UserRepository: GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<ApplicationUser>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .OrderBy(x => x.UserName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(UserRepository));
                return Enumerable.Empty<ApplicationUser>();
            }
        }

        public override Task<ApplicationUser?> GetAsync(Guid Key)
        {
            var result = dbset.FirstOrDefaultAsync(x => x.Id == Key.ToString());
            return result;
        }

        public override async Task<bool> DeleteAsync(Guid Key)
        {
            // soft delete
            var existing = await dbset.SingleOrDefaultAsync(x => x.Id == Key.ToString());
            if (existing is null)
            {
                return false;
            }

            existing.IsDeleted = true;
            return true;
        }

    }
}
