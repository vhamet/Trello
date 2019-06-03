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

        public User ValidateUserCredentials(string username, string password)
        {
            try{
                var user = db.tblUser.FirstOrDefault(u => u.Username == username);
                if (user != null && string.Equals(user.Password, password, StringComparison.Ordinal))
                {
                    return user;
                }
    
                return null;
            }
            catch (Exception e)
            {
                throw e;
                // return null;
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

        public User Create(User user)
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