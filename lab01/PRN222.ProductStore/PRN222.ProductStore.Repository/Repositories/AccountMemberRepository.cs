using Microsoft.EntityFrameworkCore;
using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Repository.Repositories
{
    public class AccountMemberRepository : GenericRepository<AccountMember>, IAccountMemberRepository
    {
        private readonly ProductStoreContext _context;
        public AccountMemberRepository(ProductStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AccountMember> GetAccountByEmail(string email, string password)
        {
            return await _context.AccountMembers.Where(x => x.EmailAddress == email && x.MemberPassword == password).FirstOrDefaultAsync();
        }
    }
}
