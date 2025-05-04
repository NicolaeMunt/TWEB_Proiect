using System;
using System.Linq;
using BusinessLogic.Data;
using Domain.Entities.User;
using Domain.Interfaces;

namespace BusinessLogic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public int CreateUser(User user, ULoginData loginData)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Add user first
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    // Set UserId on login data and add it
                    loginData.UserId = user.Id;
                    _context.LoginData.Add(loginData);
                    _context.SaveChanges();

                    transaction.Commit();
                    return user.Id;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null)
                throw new ApplicationException("User not found");

            // Update user properties
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;

            _context.SaveChanges();
        }

        public bool UpdateLoginData(ULoginData loginData)
        {
            var existingLoginData = _context.LoginData.Find(loginData.Id);
            if (existingLoginData == null)
                return false;

            // Update login data properties
            existingLoginData.PasswordHash = loginData.PasswordHash;
            existingLoginData.Salt = loginData.Salt;
            existingLoginData.LastLogin = loginData.LastLogin;

            int result = _context.SaveChanges();
            return result > 0;
        }

        public ULoginData GetLoginDataByUserId(int userId)
        {
            return _context.LoginData.FirstOrDefault(l => l.UserId == userId);
        }

        public ULoginResp CreateLoginSession(int userId, string token, DateTime expiryDate)
        {
            var session = new ULoginResp
            {
                UserId = userId,
                Token = token,
                ExpiryDate = expiryDate,
                IsActive = true
            };

            _context.LoginResponses.Add(session);
            _context.SaveChanges();
            return session;
        }

        public bool ValidateLoginSession(string token)
        {
            var session = _context.LoginResponses
                .FirstOrDefault(s => s.Token == token && s.IsActive && s.ExpiryDate > DateTime.UtcNow);
            return session != null;
        }
    }
}