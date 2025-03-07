using BusinessObjects;
using Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountMember> AuthenticateAsync(string email, string password)
        {
            var result = await _unitOfWork.AccountMembers.GetAsync(
                filter: m => m.EmailAddress == email && m.MemberPassword == password,
                pageIndex: 1,
                pageSize: 1
            );

            return result.Items.FirstOrDefault();
        }

        public async Task<AccountMember> GetByIdAsync(string memberId)
            => await _unitOfWork.AccountMembers.GetByIdAsync(memberId);

        public async Task RegisterAsync(AccountMember member)
            => await _unitOfWork.AccountMembers.AddAsync(member);
    }
}
