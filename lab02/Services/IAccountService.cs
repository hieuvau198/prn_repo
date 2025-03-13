using Repositories.Entities;
using System.Threading.Tasks;

namespace Services
{
    public interface IAccountService
    {
        Task<AccountMember> AuthenticateAsync(string email, string password);
        Task<AccountMember> GetByIdAsync(string memberId);
        Task RegisterAsync(AccountMember member);
    }
}
