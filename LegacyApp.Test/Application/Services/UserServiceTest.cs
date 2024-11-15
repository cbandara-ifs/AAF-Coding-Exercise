using AutoFixture.AutoMoq;
using AutoFixture;
using LegacyApp.Application.Services;
using Moq;
using LegacyApp.Application.Contracts.Persistence;
using LegacyApp.Application.Models;
using LegacyApp.Application.Contracts;
using LegacyApp.DataAccessLayer;
using FluentAssertions;

namespace LegacyAppTests.Application.Services
{
    public class UserServiceTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IClientRepository> _clientRepository;
        private readonly Mock<IUserCreditService> _userCreditService;
        private readonly Mock<IUserDataAccessService> _userDataAccessService;
        private readonly UserService _sut;

        public UserServiceTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            _clientRepository = _fixture.Freeze<Mock<IClientRepository>>();
            _userCreditService = _fixture.Freeze<Mock<IUserCreditService>>();
            _userDataAccessService = _fixture.Freeze<Mock<IUserDataAccessService>>();
            _sut = _fixture.Build<UserService>().OmitAutoProperties().Create();
        }

        public static IEnumerable<object[]> InvalidDateOfBirth =>
        new List<object[]>
        {
            new object[] { new DateTime(2018, 11, 12) },
            new object[] { new DateTime(2025, 1, 1) }
        };

        [Theory]
        [InlineData("","Wick")]
        [InlineData("John", "")]
        public async Task AddUser_InvalideName_ThrowsException(string firstName, string surname)
        {
            var email = _fixture.Create<string>();
            var dob = _fixture.Create<DateTime>();
            var clientId = _fixture.Create<int>();

            var result = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.AddUser(firstName, surname, email, dob, clientId));
        }

        [Theory]
        [InlineData("abc.gmail.com")]
        [InlineData("abc@gmailcom")]
        public async Task AddUser_InvalideEmail_ThrowsException(string email)
        {
            var firstName = _fixture.Create<string>();
            var surname = _fixture.Create<string>();
            var dob = _fixture.Create<DateTime>();
            var clientId = _fixture.Create<int>();

            var result = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.AddUser(firstName, surname, email, dob, clientId));
        }

        [Theory]
        [MemberData(nameof(InvalidDateOfBirth))]
        public async Task AddUser_InvalideDob_ThrowsException(DateTime dob)
        {
            var firstName = _fixture.Create<string>();
            var surname = _fixture.Create<string>();
            var email = "abc@gmail.com";
            var clientId = _fixture.Create<int>();

            var result = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.AddUser(firstName, surname, email, dob, clientId));
        }

        [Fact]
        public async Task AddUser_InsufficientCreditLimit_ThrowsException()
        {
            var client = _fixture.Build<Client>()
                                .With(opt => opt.Name, "NormalClient")
                                .With(opt => opt.Id, 1)
                                .With(opt => opt.ClientStatus, ClientStatus.Gold)
                                .Create();

            var clientId = _fixture.Create<int>();

            var creditLimit = 10;

            var firstName = _fixture.Create<string>();
            var surname = _fixture.Create<string>();
            var email = "abc@gmail.com";
            var dob = new DateTime(2000, 11, 12);

            _clientRepository.Setup(s => 
                s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(client);

            _userCreditService.Setup(s =>
                s.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(creditLimit);

            var result = await Assert.ThrowsAsync<InvalidOperationException>(async () => await _sut.AddUser(firstName, surname, email, dob, clientId));

        }

        [Fact]
        public async Task AddUser_ValidUser_succesfullyUserAdded()
        {
            var client = _fixture.Build<Client>()
                                .With(opt => opt.Name, "NormalClient")
                                .With(opt => opt.Id, 1)
                                .With(opt => opt.ClientStatus, ClientStatus.Gold)
                                .Create();

            var clientId = _fixture.Create<int>();

            var creditLimit = 500;

            var firstName = "John";
            var surname = "Wick";
            var email = "abc@gmail.com";
            var dob = new DateTime(2000, 11, 12);

            var user = _fixture.Build<User>()
                                .With(opt => opt.Id, 0)
                                .With(opt => opt.ClientId, clientId)
                                .With(opt => opt.Client, client)
                                .With(opt => opt.DateOfBirth, dob)
                                .With(opt => opt.EmailAddress, email)
                                .With(opt => opt.Firstname, firstName)
                                .With(opt => opt.Surname, surname)
                                .With(opt => opt.HasCreditLimit, true)
                                .With(opt => opt.CreditLimit, creditLimit)
                                .Create();           

            _clientRepository.Setup(s =>
                s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(client);

            _userCreditService.Setup(s =>
                s.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(creditLimit);

            _userDataAccessService.Setup(s =>
                s.AddUser(It.IsAny<User>()));

            var result = await _sut.AddUser(firstName, surname, email, dob, clientId);

            result.Should().BeEquivalentTo(user);

        }
    }
}
