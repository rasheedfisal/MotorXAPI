using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IRepository
{
    public interface ICustomerInfoRepository: IGenericRepository<OfferCustomerInfo>
    {
        Task<bool> UpdateMarkAsRead(Guid Key);
    }
}
