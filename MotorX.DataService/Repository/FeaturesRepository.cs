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
    public class FeaturesRepository: GenericRepository<Features>, IFeaturesRepository
    {
        public FeaturesRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<Features>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .Include(x => x.FeaturesType)
                    .OrderBy(x => x.FeatureName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(FeaturesRepository));
                return Enumerable.Empty<Features>();
            }
        }

        public override async Task<Features?> GetAsync(Guid Key)
        {
            try
            {
                return await dbset.Where(x => x.Id == Key)
                    .Include(x => x.FeaturesType)
                    .OrderBy(x => x.FeatureName)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAsync method has generated an error", typeof(FeaturesRepository));
                return null;
            }
        }

        public override async Task<bool> DeleteAsync(Guid Key)
        {
            // soft delete
            var existing = await dbset.SingleOrDefaultAsync(x => x.Id == Key);
            if (existing is null)
            {
                return false;
            }

            existing.IsDeleted = true;
            return true;
        }
    }
}
