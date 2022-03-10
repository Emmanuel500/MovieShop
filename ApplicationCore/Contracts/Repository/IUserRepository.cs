using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
