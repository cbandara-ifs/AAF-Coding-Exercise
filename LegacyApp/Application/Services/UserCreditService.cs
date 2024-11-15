using LegacyApp.Application.Contracts;
using System;
using System.Threading.Tasks;

namespace LegacyApp.Application.Services
{
    [System.Diagnostics.DebuggerStepThrough()]
    [System.CodeDom.Compiler.GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public partial class UserCreditServiceClient : System.ServiceModel.ClientBase<IUserCreditService>, IUserCreditService
    {

        public UserCreditServiceClient()
        {
        }

        public UserCreditServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public UserCreditServiceClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public UserCreditServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public UserCreditServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public Task<int> GetCreditLimit(string firstname, string surname, DateTime dateOfBirth)
        {
            return Channel.GetCreditLimit(firstname, surname, dateOfBirth);
        }
    }
}
