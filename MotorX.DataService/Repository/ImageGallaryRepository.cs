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
    public class ImageGallaryRepository: GenericRepository<ImageGallary>, IImageGallaryRepository
    {
        public ImageGallaryRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<ImageGallary>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .OrderBy(x => x.OrderNo)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(ImageGallaryRepository));
                return Enumerable.Empty<ImageGallary>();
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

        public async Task<IEnumerable<ImageGallary>> GetAllGallaryByCarOfferAsync(Guid Key)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false && x.CarOfferId == Key)
                    .OrderBy(x => x.OrderNo)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(ImageGallaryRepository));
                return Enumerable.Empty<ImageGallary>();
            }
        }

        public async Task<bool> DeleteGallaryByCarOffer(Guid Key)
        {
            // soft delete
            var existing = await dbset.SingleOrDefaultAsync(x => x.CarOfferId == Key);
            if (existing is null)
            {
                return false;
            }

            existing.IsDeleted = true;
            return true;
        }

        public async Task<bool> UpdateOrderAsync(Guid gallaryKey, int OrderNo)
        {
            var isExist = await dbset.SingleOrDefaultAsync(x => x.Id == gallaryKey);
            if (isExist is null)
            {
                return false;
            }
            else
            {
                isExist.OrderNo = OrderNo;
                return true;
            }

            return false;
        }
    }
}
