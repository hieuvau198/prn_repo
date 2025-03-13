using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Repository.Repositories.Interface;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Service.Services
{
    public class AccountMemberService : IAccountMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountMemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccountMember>> GetAccountMembersAsync(
         Expression<Func<AccountMember, bool>> filter = null,
         Func<IQueryable<AccountMember>, IOrderedQueryable<AccountMember>> orderBy = null,
         int? pageIndex = null,
         int? pageSize = null)
        {
            return await _unitOfWork.AccountMembers.GetAsync(filter, orderBy, pageIndex, pageSize);
        }
        public async Task UpdateAsync(AccountMember accountMember)
        {
            await _unitOfWork.AccountMembers.Update(accountMember);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await _unitOfWork.AccountMembers.GetByIdAsync(id);
            await _unitOfWork.AccountMembers.Delete(account);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<AccountMember> GetByIdAsync(int id)
        {
            return await _unitOfWork.AccountMembers.GetByIdAsync(id);
        }

        public async Task AddAsync(AccountMember accountMember)
        {
            await _unitOfWork.AccountMembers.AddAsync(accountMember);
            await _unitOfWork.CompleteAsync(); // Lưu thay đổi vào DB
        }
    }

}
