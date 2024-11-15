using LegacyApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp.DataAccessLayer
{
    public class UserDataAccessService : IUserDataAccessService
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}
