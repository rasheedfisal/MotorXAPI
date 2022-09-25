using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MotorX.DataService.Data;
using MotorX.DataService.Entities;
using MotorX.DataService.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.Repository
{
    public class CarFeaturesRepository : GenericRepository<CarFeatures>, ICarFeatureRepository
    {
        public CarFeaturesRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<CarFeatures>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .Include(x => x.Features)
                        .ThenInclude(x => x.FeaturesType)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CarFeaturesRepository));
                return Enumerable.Empty<CarFeatures>();
            }
        }

        public override async Task<IEnumerable<CarFeatures>> FindAllAsync(Expression<Func<CarFeatures, bool>> match, PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await dbset.Where(match)
                        .Include(x => x.Features)
                            .ThenInclude(x => x.FeaturesType)
                        .AsNoTracking()
                        .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                return await dbset.Where(match)
                    .Include(x => x.Features)
                        .ThenInclude(x => x.FeaturesType)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CarFeaturesRepository));
                return Enumerable.Empty<CarFeatures>();
            }
        }

        public override async Task<bool> DeleteAsync(Guid Key)
        {
            // hard delete
            try
            {
                var GetDeleted = await dbset.Where(x => x.CarOfferId == Key).AsNoTracking().ToListAsync();
                if (GetDeleted is not null && GetDeleted.Any())
                {
                    dbset.RemoveRange(GetDeleted);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} DeleteAsync method has generated an error", typeof(CarFeaturesRepository));
                return false;
            }
        }
    }

        
}
