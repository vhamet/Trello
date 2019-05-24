using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trello.Models
{
    public class UserService
    {
        private TrelloDbContext db;

        public UserService(TrelloDbContext context)
        {
            db = context;
        }

        public Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password)
        {
            try{
                var user = db.tblUser.FirstOrDefault(u => u.Username == username);
                if (user != null && string.Equals(user.Password, password, StringComparison.Ordinal))
                {
                    return Task.FromResult((true, user));
                }
    
                return Task.FromResult((false, user));
            }
            catch (Exception e)
            {
                throw e;
                // return Task.FromResult((false, (User)null));
            }
        }

        public bool CheckUserNameTaken(string username)
        {
            try{
                var user = db.tblUser.FirstOrDefault(u => u.Username == username);
                return user != null;
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }

        public bool CheckEmailTaken(string email)
        {
            try{
                var user = db.tblUser.FirstOrDefault(u => u.Email == email);
                return user != null;
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }

        public User CreateUser(User user)
        {
            try
            {
                db.tblUser.Add(user);
                db.SaveChanges();
                
                return user;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
            }
        }
    }
}