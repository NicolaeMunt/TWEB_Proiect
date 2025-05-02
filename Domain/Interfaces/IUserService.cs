using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.User;

namespace Domain.Interfaces
{

    public interface IUserService
    {
        Task<UserApp> GetUserByIdAsync(int id);
        Task<bool> RegisterUserAsync(UserApp user, string password);
        Task<(UserApp user, string token)> AuthenticateAsync(string username, string password);
        Task<bool> ValidateTokenAsync(string token);
        Task UpdateUserAsync(UserApp user);
    }
}
