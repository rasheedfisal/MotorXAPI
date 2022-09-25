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
    public class CarTypeRepository: GenericRepository<Cartype>, ICarTypeRepository
    {
        public CarTypeRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public override async Task<IEnumerable<Cartype>> GetAllAsync(PaginationFilter? paginationFilter = null)
        {
            try
            {
                return await dbset.Where(x => x.IsDeleted == false)
                    .OrderBy(x => x.TypeName)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All method has generated an error", typeof(CarTypeRepository));
                return Enumerable.Empty<Cartype>();
            }
        }

        public override async Task<Cartype?> UpdateAsync(Cartype entity, Guid Key)
        {
            if (entity is null)
                return null;

            var existing = await dbset.FindAsync(Key);

            if (existing is not null)
            {
                existing.TypeName = entity.TypeName;
                existing.TypeNameAr = entity.TypeNameAr;

                if (entity.ImgPath is not null)
                {
                    existing.ImgPath = entity.ImgPath;
                }

            }

            return entity;
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
    }
}
