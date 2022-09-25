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
    public class RefreshTokensRepository: GenericRepository<RefreshToken>, IRefreshTokensRepository
    {
        public RefreshTokensRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<RefreshToken>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(RefreshTokensRepository));
                return Enumerable.Empty<RefreshToken>();
            }
        }

        public async Task<RefreshToken?> GetByRefreshToken(string refreshToken)
        {
            try
            {
                return await dbset.Where(x => x.Token.ToLower() == refreshToken.ToLower())
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetByRefreshToken method has generated an error", typeof(RefreshTokensRepository));
                return null;
            }
        }

        public async Task<bool> MarkRefreshTokenAsRevoked(RefreshToken refreshToken)
        {
            try
            {
                var token = await dbset.SingleOrDefaultAsync(x => x.Token.ToLower() == refreshToken.Token.ToLower());

                if (token is null) return false;
                token.IsRevoked = refreshToken.IsRevoked;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} MarkRefreshTokenAsRevoked method has generated an error", typeof(RefreshTokensRepository));
                return false;
            }
        }

        public async Task<bool> MarkRefreshTokenAsUsed(RefreshToken refreshToken)
        {
            try
            {
                var token = await dbset.SingleOrDefaultAsync(x => x.Token.ToLower() == refreshToken.Token.ToLower());

                if (token is null) return false;
                token.IsUsed = refreshToken.IsUsed;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} MarkRefreshTokenAsUsed method has generated an error", typeof(RefreshTokensRepository));
                return false;
            }
        }
    }
}
