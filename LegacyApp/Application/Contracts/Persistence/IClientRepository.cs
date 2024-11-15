using LegacyApp.Application.Models;
using System.Threading.Tasks;

namespace LegacyApp.Application.Contracts.Persistence
{
    public interface IClientRepository
    {
        public Task<Client> GetByIdAsync(int id);
    }
}
