using ApplicationCore.Contracts.Repository;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class EfRepository<T>: IRepository<T> where T : class
    {
        protected readonly MovieShopDbContext _dbContext;

        public EfRepository(MovieShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual Task<T> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
