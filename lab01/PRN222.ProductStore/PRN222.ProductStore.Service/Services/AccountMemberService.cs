using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using PRN222.ProductStore.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services
{
    public class AccountMemberService : IAccountMemberService
    {
        private readonly IAccountMemberRepository _accountMemberRepository;

        public AccountMemberService(IAccountMemberRepository accountMemberRepository)
        {
            _accountMemberRepository = accountMemberRepository;
        }

        public async Task<AccountMember> GetAccountByEmail(string email, string password)
        {
            return await _accountMemberRepository.GetAccountByEmail(email, password);
        }
    }
}
