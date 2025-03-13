using Repositories.Entities;
using Repositories.Interfaces;
using Services.BusinessModels;
using Services.Interfaces;
using System.Threading.Tasks;

namespace Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountModel?> AuthenticateAsync(string email, string password)
        {
            AccountMember? result = await _unitOfWork.AccountMembers.FindAsync(
                predicate: m => m.EmailAddress == email && m.MemberPassword == password
            );

            return result != null ? MapToModel(result) : null;
        }

        public async Task<AccountModel?> GetByIdAsync(int memberId)
        {
            AccountMember? entity = await _unitOfWork.AccountMembers.GetByIdAsync(memberId);
            return entity != null ? MapToModel(entity) : null;
        }

        public async Task RegisterAsync(AccountModel model)
        {
            var entity = MapToEntity(model);
            await _unitOfWork.AccountMembers.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        private static AccountModel MapToModel(AccountMember entity)
        {
            return new AccountModel
            {
                MemberId = entity.MemberId,
                MemberPassword = entity.MemberPassword,
                FullName = entity.FullName,
                EmailAddress = entity.EmailAddress,
                MemberRole = entity.MemberRole
            };
        }

        private static AccountMember MapToEntity(AccountModel model)
        {
            return new AccountMember
            {
                MemberId = model.MemberId,
                MemberPassword = model.MemberPassword,
                FullName = model.FullName,
                EmailAddress = model.EmailAddress,
                MemberRole = model.MemberRole
            };
        }
    }
}
