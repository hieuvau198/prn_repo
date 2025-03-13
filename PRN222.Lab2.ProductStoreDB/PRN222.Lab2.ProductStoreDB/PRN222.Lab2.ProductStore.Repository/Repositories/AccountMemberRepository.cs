using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Repository.Repositories.Interface;

namespace PRN222.Lab2.ProductStore.Repository.Repositories
{
    public class AccountMemberRepository : GenericRepository<AccountMember>, IAccountMemberRepository
    {
        public AccountMemberRepository(MyStoreDbContext context) : base(context) { }
    }

}
