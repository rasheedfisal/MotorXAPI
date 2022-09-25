using MotorX.DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorX.DataService.IRepository
{
    public interface IRefreshTokensRepository: IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByRefreshToken(string refreshToken);
        Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken);
        Task<bool> MarkRefreshTokenAsRevoked(RefreshToken refreshToken);
    }
}
