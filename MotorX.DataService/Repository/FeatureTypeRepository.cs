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
    public class FeatureTypeRepository: GenericRepository<FeaturesType>, IFeatureTypeRepository
    {
        public FeatureTypeRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<FeaturesType>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .OrderBy(x => x.TypeName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(FeatureTypeRepository));
                return Enumerable.Empty<FeaturesType>();
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
