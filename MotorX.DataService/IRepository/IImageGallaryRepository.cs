using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IRepository
{
    public interface IImageGallaryRepository: IGenericRepository<ImageGallary>
    {
        Task<IEnumerable<ImageGallary>> GetAllGallaryByCarOfferAsync(Guid Key);
        Task<bool> DeleteGallaryByCarOffer(Guid Key);
        Task<bool> UpdateOrderAsync(Guid gallaryKey, int OrderNo);
    }
}
