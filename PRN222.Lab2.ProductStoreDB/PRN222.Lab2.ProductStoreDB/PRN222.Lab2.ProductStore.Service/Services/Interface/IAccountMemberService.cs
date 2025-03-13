using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;

namespace PRN222.Lab2.ProductStore.Service.Services.Interface
{
    public interface IAccountMemberService
    {
        Task<IEnumerable<AccountMember>> GetAccountMembersAsync(
        Expression<Func<AccountMember, bool>> filter = null,
        Func<IQueryable<AccountMember>, IOrderedQueryable<AccountMember>> orderBy = null,
        int? pageIndex = null,
        int? pageSize = null);

        Task<AccountMember> GetByIdAsync(int id);
        Task AddAsync(AccountMember accountMember);
        Task UpdateAsync(AccountMember accountMember);
        Task DeleteAsync(int id);
    }

}
