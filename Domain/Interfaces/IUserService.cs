// Domain/Interfaces/IUserService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.User;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        // Existing methods
        User GetUserById(int id);
        bool RegisterUser(User user, string password);
        User Authenticate(string username, string password, out string token);
        bool ValidateToken(string token);
        void UpdateUser(User user);

        // New admin methods
        IEnumerable<User> GetAllUsers();
        bool PromoteToAdmin(int userId);
        bool DemoteFromAdmin(int userId);
        bool DeleteUser(int userId);
        bool IsUserAdmin(int userId);
        bool IsUserAdmin(string token);
    }
}