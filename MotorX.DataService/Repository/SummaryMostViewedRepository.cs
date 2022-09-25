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
    public class SummaryMostViewedRepository: GenericRepository<SummaryMostViewed>, ISummeryMostViewedRepository
    {
        public SummaryMostViewedRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<SummaryMostViewed>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .Include(x => x.CarOffer)
                    .ThenInclude(x => x.BrandModel)
                    .ThenInclude(x => x.Brand)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAllAsync method has generated an error", typeof(SummaryMostViewedRepository));
                return Enumerable.Empty<SummaryMostViewed>();
            }
        }

        public override async Task<IEnumerable<SummaryMostViewed>> FindAllAsync(Expression<Func<SummaryMostViewed, bool>> match, PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await dbset.Where(match)
                    .Include(x => x.CarOffer)
                    .ThenInclude(x => x.BrandModel)
                    .ThenInclude(x => x.Brand)
                    .AsNoTracking()
                    .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

                return await dbset.Where(match)
                    .Include(x => x.CarOffer)
                    .ThenInclude(x => x.BrandModel)
                    .ThenInclude(x => x.Brand)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} FindAllAsync method has generated an error", typeof(SummaryMostViewedRepository));
                return Enumerable.Empty<SummaryMostViewed>();
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

        public async Task<bool> UpsertAsync(Guid offerId)
        {
            var isExist = await dbset.SingleOrDefaultAsync(x => x.CarOfferId == offerId);
            if (isExist is null)
            {
                await dbset.AddAsync(new SummaryMostViewed
                {
                    CarOfferId = offerId
                });
                return true;
            }
            else
            {
                isExist.NumberOfViews += 1;
                return true;
            }
            
            return false;
        }
    }
}
