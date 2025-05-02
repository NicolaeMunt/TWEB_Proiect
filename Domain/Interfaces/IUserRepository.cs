using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.User;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserApp> GetByIdAsync(int id);
        Task<UserApp> GetByUsernameAsync(string username);
        Task<UserApp> GetByEmailAsync(string email);
        Task<int> CreateUserAsync(UserApp user, ULoginDataApp loginData);
        Task UpdateUserAsync(UserApp user);
        Task<bool> UpdateLoginDataAsync(ULoginDataApp loginData);
        Task<ULoginDataApp> GetLoginDataByUserIdAsync(int userId);
        Task<ULoginRespApp> CreateLoginSessionAsync(int userId, string token, DateTime expiryDate);
        Task<bool> ValidateLoginSessionAsync(string token);
    }
}
