using System;
using Domain.Entities.User;
using Domain.Interfaces;
using Helpers.Security;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        public bool RegisterUser(User user, string password)
        {
            // Check if user already exists
            if (_userRepository.GetByEmail(user.Email) != null)
            {
                throw new ApplicationException("Email already registered");
            }

            if (_userRepository.GetByUsername(user.Username) != null)
            {
                throw new ApplicationException("Username already taken");
            }

            // Create password hash and salt
            PasswordHelper.CreatePasswordHash(password, out string passwordHash, out string salt);

            // Create login data
            var loginData = new ULoginData
            {
                PasswordHash = passwordHash,
                Salt = salt,
                LastLogin = DateTime.UtcNow
            };

            // Add user and login data
            int userId = _userRepository.CreateUser(user, loginData);

            return userId > 0;
        }

        public User Authenticate(string username, string password, out string token)
        {
            token = null;

            var user = _userRepository.GetByUsername(username);
            if (user == null)
                return null;

            // Get login data
            var loginData = _userRepository.GetLoginDataByUserId(user.Id);
            if (loginData == null)
                return null;

            // Verify password
            if (!PasswordHelper.VerifyPasswordHash(password, loginData.PasswordHash, loginData.Salt))
                return null;

            // Generate auth token
            token = TokenHelper.GenerateToken();
            DateTime expiryDate = TokenHelper.GetTokenExpiry();

            // Create login session
            _userRepository.CreateLoginSession(user.Id, token, expiryDate);

            // Update last login time
            loginData.LastLogin = DateTime.UtcNow;
            _userRepository.UpdateLoginData(loginData);

            return user;
        }

        public bool ValidateToken(string token)
        {
            return _userRepository.ValidateLoginSession(token);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }
    }
}
