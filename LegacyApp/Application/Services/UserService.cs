using LegacyApp.Application.Contracts;
using LegacyApp.Application.Contracts.Persistence;
using LegacyApp.Application.Extensions;
using LegacyApp.Application.Models;
using LegacyApp.DataAccessLayer;
using System;
using System.Threading.Tasks;

namespace LegacyApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserCreditService _userCreditService;
        private readonly IClientRepository _clientRepository;
        private readonly IUserDataAccessService _userDataAccessService;

        public UserService(IUserCreditService userCreditService, IClientRepository clientRepository, IUserDataAccessService userDataAccessService)
        {
            _userCreditService = userCreditService;
            _clientRepository = clientRepository;
            _userDataAccessService = userDataAccessService;
        }
        public async Task<User> AddUser(string firstName, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            ValidateUser(firstName, surname, email, dateOfBirth);

            var client = await _clientRepository.GetByIdAsync(clientId);

            var user = new User
            {
                Client = client,
                ClientId = clientId,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstName,
                Surname = surname,
                HasCreditLimit = false
            };

            await UpdateUserCreditLimitAsync(user);

            ValidateCreditLimit(user.HasCreditLimit, user?.CreditLimit);

            _userDataAccessService.AddUser(user);

            return user;
        }

        public async Task UpdateUserCreditLimitAsync(User user)
        {
            if (user.Client.Name != "VeryImportantClient")
            {
                user.HasCreditLimit = true;
                var creditLimit = await _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                user.CreditLimit = user.Client.GetCreditLimit(creditLimit);
            }
        }

        private void ValidateUser(string firstName, string surname, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname))
            {
                throw new InvalidOperationException("user firstname / surname is required ");
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                throw new InvalidOperationException("user email is invalid ");
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day) age--;

            if (age < 21)
            {
                throw new InvalidOperationException("user should be older than 21 years");
            }
        }

        private void ValidateCreditLimit(bool hasCreditLimit, int? creditLimit)
        {
            if (hasCreditLimit && creditLimit < 500)
            {
                throw new InvalidOperationException("insufficient credit limit");
            }
        }
    }
}
