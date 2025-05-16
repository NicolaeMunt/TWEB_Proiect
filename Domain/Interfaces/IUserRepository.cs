using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.User;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        // Existing methods
        User GetById(int id);
        User GetByUsername(string username);
        User GetByEmail(string email);
        int CreateUser(User user, ULoginData loginData);
        void UpdateUser(User user);
        bool UpdateLoginData(ULoginData loginData);
        ULoginData GetLoginDataByUserId(int userId);
        ULoginResp CreateLoginSession(int userId, string token, System.DateTime expiryDate);
        bool ValidateLoginSession(string token);

        // New admin-related methods
        IEnumerable<User> GetAllUsers();
        bool PromoteToAdmin(int userId);
        bool DemoteFromAdmin(int userId);
        bool DeleteUser(int userId);
        User GetUserByToken(string token);
    }
}