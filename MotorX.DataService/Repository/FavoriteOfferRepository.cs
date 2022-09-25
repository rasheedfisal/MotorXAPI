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
    public class FavoriteOfferRepository: GenericRepository<FavoriteOffer>, IFavoriteOfferRepository
    {
        public FavoriteOfferRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<FavoriteOffer>> GetMAllOfferAsync(string UserKey, PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await dbset
                   .Include(x => x.Offer.BrandModel)
                       .ThenInclude(x => x.Brand)
                   .Include(x => x.Offer.Year)
                   .Include(x => x.Offer.Currency)
                    .Include(x => x.Offer.Cartype)
                     .Include(x => x.Offer.ImageGallaries)
                   .OrderByDescending(x => x.Offer.AddedDate)
                   .AsNoTracking()
                   .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                return await dbset
                      .Include(x => x.Offer.BrandModel)
                       .ThenInclude(x => x.Brand)
                   .Include(x => x.Offer.Year)
                   .Include(x => x.Offer.Currency)
                    .Include(x => x.Offer.Cartype)
                     .Include(x => x.Offer.ImageGallaries)
                   .OrderByDescending(x => x.Offer.AddedDate)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetMAllOfferAsync method has generated an error", typeof(FavoriteOfferRepository));
                return Enumerable.Empty<FavoriteOffer>();
            }
        }

        public async Task<FavoriteOffer?> GetMOfferDetailsAsync(Guid key)
        {
            try
            {
                return await dbset.Where(x => x.Offer.Id == key)
                    .Include(x => x.Offer.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Offer.Gearbox)
                    .Include(x => x.Offer.Trim)
                    .Include(x => x.Offer.Specs)
                    .Include(x => x.Offer.Year)
                    .Include(x => x.Offer.Currency)
                    .Include(x => x.Offer.ImageGallaries.OrderBy(y => y.OrderNo))
                   .Include(x => x.Offer.CarFeatures)
                       .ThenInclude(x => x.Features)
                        .ThenInclude(x => x.FeaturesType)
                            .OrderByDescending(x => x.Offer.AddedDate)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetMGetOfferAsync method has generated an error", typeof(FavoriteOfferRepository));
                return null;
            }
        }

        public async Task<bool> UpsertAsync(Guid offerId, string ClientId, bool IsFavorite)
        {
            var isExist = await dbset.SingleOrDefaultAsync(x => x.OfferId == offerId && x.UserId == ClientId);
            if (isExist is null)
            {
                await dbset.AddAsync(new FavoriteOffer
                {
                    OfferId = offerId,
                    UserId = ClientId,
                    IsFavorite = IsFavorite
                });
                return true;
            }
            else
            {
                isExist.IsFavorite = IsFavorite;
                return true;
            }

            return false;
        }
    }
}
