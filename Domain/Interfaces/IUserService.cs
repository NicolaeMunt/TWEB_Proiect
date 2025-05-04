// Domain/Interfaces/IUserService.cs
using System.Threading.Tasks;
using Domain.Entities.User;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        User GetUserById(int id);
        bool RegisterUser(User user, string password);
        User Authenticate(string username, string password, out string token);
        bool ValidateToken(string token);
        void UpdateUser(User user);
    }
}