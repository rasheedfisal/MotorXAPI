using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IRepository
{
    public interface ICarOfferRepository: IGenericRepository<CarOffer>
    {
        Task<IEnumerable<CarOffer>> GetMAllOfferAsync(Expression<Func<CarOffer, bool>>? ExtraConditions = null, string? UserKey = null, PaginationFilter? paginationFilter = null);
        Task<CarOffer?> GetMOfferDetailsAsync(Guid key, string? UserKey = null);
        Task<IEnumerable<CarOffer>> GetMAllOfferDevAsync(Expression<Func<CarOffer, bool>>? ExtraConditions = null, string ? UserKey = null, PaginationFilter? paginationFilter = null);
        Task<IEnumerable<CarOffer>> FindMAllAsync(Expression<Func<CarOffer, bool>> match, PaginationFilter? paginationFilter = null);
    }
}
