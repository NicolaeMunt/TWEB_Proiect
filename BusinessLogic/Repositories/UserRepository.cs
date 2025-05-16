using System;
using System.Collections.Generic;
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

                    // Force save to get the user ID
                    _context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"User saved with ID: {user.Id}");

                    // Set UserId on login data and add it
                    loginData.UserId = user.Id;
                    _context.LoginData.Add(loginData);

                    // Force save to add login data
                    _context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Login data saved with ID: {loginData.Id}");

                    // Commit transaction
                    transaction.Commit();
                    System.Diagnostics.Debug.WriteLine("Transaction committed successfully");

                    return user.Id;
                }
                catch (Exception ex)
                {
                    // Log exception details
                    System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                    // Rollback transaction
                    transaction.Rollback();
                    System.Diagnostics.Debug.WriteLine("Transaction rolled back");

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

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public bool PromoteToAdmin(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return false;

            user.IsAdmin = true;
            _context.SaveChanges();
            return true;
        }

        public bool DemoteFromAdmin(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return false;

            user.IsAdmin = false;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

        public User GetUserByToken(string token)
        {
            var loginResp = _context.LoginResponses
                .FirstOrDefault(l => l.Token == token && l.IsActive && l.ExpiryDate > DateTime.UtcNow);

            if (loginResp == null)
                return null;

            return _context.Users.Find(loginResp.UserId);
        }
    }
}