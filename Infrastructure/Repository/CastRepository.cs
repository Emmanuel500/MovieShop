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

        public override Cast GetById(int id)
        {
            var castDetails = _dbContext.Casts.Include(c => c.Name)
                .Include(c => c.Gender)
                .Include(c => c.TmdbUrl)
                .Include(c => c.ProfilePath)
                .Include(c => c.MovieCasts).ThenInclude(c => c.Movie)
                .FirstOrDefault(m => m.Id == id);
            return castDetails;
        }
        
    }
}
