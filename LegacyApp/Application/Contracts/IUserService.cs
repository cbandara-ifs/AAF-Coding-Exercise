using System;
using System.Threading.Tasks;
using LegacyApp.Application.Models;

namespace LegacyApp.Application.Contracts
{
    public interface IUserService
    {
        public Task<User> AddUser(string firstName, string surname, string email, DateTime dateOfBirth, int clientId);

        public Task UpdateUserCreditLimitAsync(User user);

    }
}
