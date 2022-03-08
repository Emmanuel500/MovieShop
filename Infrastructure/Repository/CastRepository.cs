using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CastRepository : EfRepository<Cast>, ICastRepository
    {
        public CastRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Cast> GetById(int id)
        {
            var castDetails = await _dbContext.Casts.Include(c => c.MovieCasts).ThenInclude(c => c.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            return castDetails;
        }
        
    }
}
