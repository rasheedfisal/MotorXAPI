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
    public class CarOfferRepository : GenericRepository<CarOffer>, ICarOfferRepository
    {
        public CarOfferRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<CarOffer>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await dbset.Where(x => x.IsDeleted == false)
                   .Include(x => x.BrandModel)
                       .ThenInclude(x => x.Brand)
                   .Include(x => x.Trim)
                   .Include(x => x.Year)
                   .Include(x => x.Colors)
                   .Include(x => x.Cartype)
                   .Include(x => x.Gearbox)
                   .Include(x => x.Specs)
                   .Include(x => x.Location)
                   .Include(x => x.Currency)
                    .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                   .Include(x => x.CarFeatures)
                       .ThenInclude(x => x.Features)
                        .ThenInclude(x => x.FeaturesType)
                   .OrderByDescending(x => x.AddedDate)
                   .AsNoTracking()
                   .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                return await dbset.Where(x => x.IsDeleted == false)
                    .Include(x => x.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Trim)
                    .Include(x => x.Year)
                    .Include(x => x.Colors)
                    .Include(x => x.Cartype)
                    .Include(x => x.Gearbox)
                    .Include(x => x.Specs)
                    .Include(x => x.Location)
                    .Include(x => x.Currency)
                     .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                    .Include(x => x.CarFeatures)
                                .ThenInclude(x => x.Features)
                            .ThenInclude(x => x.FeaturesType)
                    .OrderByDescending(x => x.AddedDate)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CarOfferRepository));
                return Enumerable.Empty<CarOffer>();
            }
        }

        public override async Task<IEnumerable<CarOffer>> FindAllAsync(Expression<Func<CarOffer, bool>> match, PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await dbset.Where(match)
                   .Include(x => x.BrandModel)
                       .ThenInclude(x => x.Brand)
                   .Include(x => x.Trim)
                   .Include(x => x.Year)
                   .Include(x => x.Colors)
                   .Include(x => x.Cartype)
                   .Include(x => x.Gearbox)
                   .Include(x => x.Specs)
                   .Include(x => x.Location)
                   .Include(x => x.Currency)
                    .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                   .Include(x => x.CarFeatures)
                       .ThenInclude(x => x.Features)
                        .ThenInclude(x => x.FeaturesType)
                   .OrderByDescending(x => x.AddedDate)
                   .AsNoTracking()
                   .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                return await dbset.Where(match)
                    .Include(x => x.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Trim)
                    .Include(x => x.Year)
                    .Include(x => x.Colors)
                    .Include(x => x.Cartype)
                    .Include(x => x.Gearbox)
                    .Include(x => x.Specs)
                    .Include(x => x.Location)
                    .Include(x => x.Currency)
                     .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                    .Include(x => x.CarFeatures)
                                .ThenInclude(x => x.Features)
                            .ThenInclude(x => x.FeaturesType)
                    .OrderByDescending(x => x.AddedDate)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CarOfferRepository));
                return Enumerable.Empty<CarOffer>();
            }
        }

        public override async Task<CarOffer?> GetAsync(Guid Key)
        {
            try
            {
                return await dbset.Where(x => x.Id == Key)
                    .Include(x => x.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Trim)
                    .Include(x => x.Year)
                    .Include(x => x.Colors)
                    .Include(x => x.Cartype)
                    .Include(x => x.Gearbox)
                    .Include(x => x.Specs)
                    .Include(x => x.Location)
                    .Include(x => x.Currency)
                     .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                    .Include(x => x.CarFeatures)
                        .ThenInclude(x => x.Features)
                            .ThenInclude(x => x.FeaturesType)
                    .OrderByDescending(x => x.AddedDate)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CarOfferRepository));
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

        public async Task<IEnumerable<CarOffer>> GetMAllOfferAsync(Expression<Func<CarOffer, bool>>? ExtraConditions = null, string? UserKey = null, PaginationFilter? paginationFilter = null)
        {
            try
            {
                //if (paginationFilter is null && UserKey is null)
                //{
                //    return await dbset.Where(x => x.IsDeleted == false && x.IsActive == true)
                //   .Include(x => x.BrandModel)
                //       .ThenInclude(x => x.Brand)
                //   .Include(x => x.Year)
                //   .Include(x => x.Currency)
                //    .Include(x => x.Cartype)
                //     .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //   .OrderByDescending(x => x.AddedDate)
                //   .AsNoTracking()
                //   .ToListAsync();
                //}

                //if (paginationFilter is null && UserKey is not null)
                //{
                //    return await dbset.Where(x => x.IsDeleted == false && x.IsActive == true)
                //   .Include(x => x.BrandModel)
                //       .ThenInclude(x => x.Brand)
                //   .Include(x => x.Year)
                //   .Include(x => x.Currency)
                //    .Include(x => x.Cartype)
                //      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //     .Include(x => x.Favorite.Where(x => x.UserId == UserKey))
                //   .OrderByDescending(x => x.AddedDate)
                //   .AsNoTracking()
                //   .ToListAsync();
                //}

                //if (paginationFilter is not null && UserKey is null)
                //{
                //    return await dbset.Where(x => x.IsDeleted == false && x.IsActive == true)
                //    .Include(x => x.BrandModel)
                //        .ThenInclude(x => x.Brand)
                //    .Include(x => x.Year)
                //    .Include(x => x.Currency)
                //     .Include(x => x.Cartype)
                //           .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //    .OrderByDescending(x => x.AddedDate)
                //    .Skip(skip)
                //    .Take(paginationFilter.PageSize)
                //    .AsNoTracking()
                //    .ToListAsync();
                //}


                //return await dbset.Where(x => x.IsDeleted == false && x.IsActive == true)
                //    .Include(x => x.BrandModel)
                //        .ThenInclude(x => x.Brand)
                //    .Include(x => x.Year)
                //    .Include(x => x.Currency)
                //     .Include(x => x.Cartype)
                //      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //     .Include(x => x.Favorite.Where(x => x.UserId == UserKey))
                //    .OrderByDescending(x => x.AddedDate)
                //    .Skip(skip)
                //    .Take(paginationFilter.PageSize)
                //    .AsNoTracking()
                //    .ToListAsync();

                var query = dbset.Where(x => x.IsDeleted == false && x.IsActive == true);

                if (ExtraConditions != null)
                {
                    query = query.Where(ExtraConditions);
                }

                query = query
                   .Include(x => x.BrandModel)
                       .ThenInclude(x => x.Brand)
                   .Include(x => x.Year)
                   .Include(x => x.Currency)
                    .Include(x => x.Cartype)
                     .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo));
                if (UserKey is not null)
                {
                    query = query.Include(x => x.Favorite.Where(x => x.UserId == UserKey));
                }

                if (paginationFilter is not null)
                {
                    var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

                    query = query.Skip(skip)
                    .Take(paginationFilter.PageSize);
                }

                var result = await query
                            .OrderByDescending(x => x.AddedDate)
                            .AsNoTracking()
                            .ToListAsync()
                            .ConfigureAwait(false);

                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetMAllOfferAsync method has generated an error", typeof(CarOfferRepository));
                return Enumerable.Empty<CarOffer>();
            }
        }

        public async Task<IEnumerable<CarOffer>> GetMAllOfferDevAsync(Expression<Func<CarOffer, bool>>? ExtraConditions = null, string ? UserKey = null, PaginationFilter? paginationFilter = null)
        {
            try
            {
                //if (paginationFilter is null && UserKey is null)
                //{
                //    return await dbset.Where(x => x.IsDeleted == false)
                //   .Include(x => x.BrandModel)
                //       .ThenInclude(x => x.Brand)
                //   .Include(x => x.Year)
                //   .Include(x => x.Currency)
                //    .Include(x => x.Cartype)
                //      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //   .OrderByDescending(x => x.AddedDate)
                //   .AsNoTracking()
                //   .ToListAsync();
                //}

                //if (paginationFilter is null && UserKey is not null)
                //{
                //    return await dbset.Where(x => x.IsDeleted == false)
                //   .Include(x => x.BrandModel)
                //       .ThenInclude(x => x.Brand)
                //   .Include(x => x.Year)
                //   .Include(x => x.Currency)
                //    .Include(x => x.Cartype)
                //      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //     .Include(x => x.Favorite.Where(x => x.UserId == UserKey))
                //   .OrderByDescending(x => x.AddedDate)
                //   .AsNoTracking()
                //   .ToListAsync();
                //}
                //var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                //if (paginationFilter is not null && UserKey is null)
                //{
                //    return await dbset.Where(x => x.IsDeleted == false)
                //    .Include(x => x.BrandModel)
                //        .ThenInclude(x => x.Brand)
                //    .Include(x => x.Year)
                //    .Include(x => x.Currency)
                //     .Include(x => x.Cartype)
                //      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //    .OrderByDescending(x => x.AddedDate)
                //    .Skip(skip)
                //    .Take(paginationFilter.PageSize)
                //    .AsNoTracking()
                //    .ToListAsync();
                //}


                //return await dbset.Where(x => x.IsDeleted == false)
                //    .Include(x => x.BrandModel)
                //        .ThenInclude(x => x.Brand)
                //    .Include(x => x.Year)
                //    .Include(x => x.Currency)
                //     .Include(x => x.Cartype)
                //      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                //     .Include(x => x.Favorite.Where(x => x.UserId == UserKey))
                //    .OrderByDescending(x => x.AddedDate)
                //    .Skip(skip)
                //    .Take(paginationFilter.PageSize)
                //    .AsNoTracking()
                //    .ToListAsync();

                var query = dbset.Where(x => x.IsDeleted == false);

                if (ExtraConditions != null)
                {
                    query = query.Where(ExtraConditions);
                }

                query = query
                    .Include(x => x.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Year)
                    .Include(x => x.Currency)
                     .Include(x => x.Cartype)
                      .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo));
                if (UserKey is not null)
                {
                    query = query.Include(x => x.Favorite.Where(x => x.UserId == UserKey));
                }

                if (paginationFilter is not null)
                {
                    var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                    query = query
                    .Skip(skip)
                    .Take(paginationFilter.PageSize);
                }

                var result = await query
                .OrderByDescending(x => x.AddedDate)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetMAllOfferAsync method has generated an error", typeof(CarOfferRepository));
                return Enumerable.Empty<CarOffer>();
            }
        }

        public async Task<CarOffer?> GetMOfferDetailsAsync(Guid key, string? UserKey = null)
        {
            try
            {
                return await dbset.Where(x => x.Id == key)
                    .Include(x => x.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Gearbox)
                    .Include(x => x.Trim)
                    .Include(x => x.Specs)
                    .Include(x => x.Colors)
                    .Include(x => x.Year)
                    .Include(x => x.Location)
                    .Include(x => x.Currency)
                     .Include(x => x.Cartype)
                     .Include(x => x.ImageGallaries.Where(p => p.IsDeleted == false).OrderBy(s => s.OrderNo))
                   .Include(x => x.CarFeatures)
                       .ThenInclude(x => x.Features)
                        .ThenInclude(x => x.FeaturesType)
                            .OrderByDescending(x => x.AddedDate)
                    .Include(x => x.Favorite.Where(x => x.UserId == UserKey))
                    .Include(x => x.AppUser)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetMGetOfferAsync method has generated an error", typeof(CarOfferRepository));
                return null;
            }
        }

        public async Task<IEnumerable<CarOffer>> FindMAllAsync(Expression<Func<CarOffer, bool>> match, PaginationFilter? paginationFilter = null)
        {
            try
            {
                if (paginationFilter is null)
                {
                    return await _context.Set<CarOffer>().Where(match)
                   .Include(x => x.BrandModel)
                       .ThenInclude(x => x.Brand)
                   .Include(x => x.Year)
                   .Include(x => x.Currency)
                   .Include(x => x.Cartype)
                   .Include(x => x.Favorite)
                   .OrderByDescending(x => x.AddedDate)
                   .AsNoTracking()
                   .ToListAsync();
                }

                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

                return await _context.Set<CarOffer>().Where(match)
                    .Include(x => x.BrandModel)
                        .ThenInclude(x => x.Brand)
                    .Include(x => x.Year)
                    .Include(x => x.Currency)
                    .Include(x => x.Cartype)
                    .Include(x => x.Favorite)
                    .OrderByDescending(x => x.AddedDate)
                    .Skip(skip)
                    .Take(paginationFilter.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} FindMAllAsync method has generated an error", typeof(CarOfferRepository));
                return null;
            }
        }

    }
}
