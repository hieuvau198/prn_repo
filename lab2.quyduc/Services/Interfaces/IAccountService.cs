using Services.BusinessModels;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountModel?> AuthenticateAsync(string email, string password);
        Task<AccountModel?> GetByIdAsync(int memberId);
        Task RegisterAsync(AccountModel model);
    }
}
