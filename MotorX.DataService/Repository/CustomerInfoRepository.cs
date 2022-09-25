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
    public class CustomerInfoRepository: GenericRepository<OfferCustomerInfo>, ICustomerInfoRepository
    {
        public CustomerInfoRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<OfferCustomerInfo>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await dbset.Where(x => x.IsDeleted == false)
                    .Include(x => x.Offer)
                    .ThenInclude(y => y.BrandModel)
                        .ThenInclude(c => c.Brand)
                    .Include(x => x.Offer.Year)
                    .Include(x => x.Offer.Currency)
                    .OrderBy(x => x.MarkAsRead)
                    .OrderByDescending(x => x.AddedDate)
                    .AsNoTracking()
                    .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                return await dbset.Where(x => x.IsDeleted == false)
                   .Include(x => x.Offer)
                   .ThenInclude(y => y.BrandModel)
                       .ThenInclude(c => c.Brand)
                   .Include(x => x.Offer.Year)
                   .Include(x => x.Offer.Currency)
                   .OrderBy(x => x.MarkAsRead)
                   .OrderByDescending(x => x.AddedDate)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                   .AsNoTracking()
                   .ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CustomerInfoRepository));
                return Enumerable.Empty<OfferCustomerInfo>();
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

        public async Task<bool> UpdateMarkAsRead(Guid Key)
        {
            // soft delete
            var existing = await dbset.SingleOrDefaultAsync(x => x.Id == Key);
            if (existing is null)
            {
                return false;
            }

            existing.MarkAsRead = true;
            return true;
        }
    }
}
