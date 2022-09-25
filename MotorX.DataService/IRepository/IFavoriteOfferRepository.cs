using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IRepository
{
    public interface IFavoriteOfferRepository: IGenericRepository<FavoriteOffer>
    {
        Task<IEnumerable<FavoriteOffer>> GetMAllOfferAsync(string UserKey, PaginationFilter? paginationFilter = null);
        Task<FavoriteOffer?> GetMOfferDetailsAsync(Guid key);
        Task<bool> UpsertAsync(Guid offerId, string ClientId, bool IsFavorite);
    }
}
