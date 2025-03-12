using AutoMapper;
using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Repository.Repositories.Interfaces;
using PRN222.ProductStore.Service.DTOs;
using PRN222.ProductStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services
{
    public class AccountMemberService : IAccountMemberService
    {
        private readonly IGenericRepository<AccountMember> _accountRepository;
        private readonly IMapper _mapper;

        public AccountMemberService(IGenericRepository<AccountMember> accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountMemberDto>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAsync();
            return _mapper.Map<IEnumerable<AccountMemberDto>>(accounts);
        }

        public async Task<AccountMemberDto> GetAccountByIdAsync(string memberId)
        {
            var account = await _accountRepository.GetByIdAsync(memberId);
            return _mapper.Map<AccountMemberDto>(account);
        }

        public async Task<AccountMemberDto> GetAccountByEmailAsync(string email)
        {
            var account = await _accountRepository.FindAsync(a => a.EmailAddress == email);
            return _mapper.Map<AccountMemberDto>(account.FirstOrDefault());
        }

        public async Task RegisterAccountAsync(RegisterAccountDto registerDto)
        {
            var account = _mapper.Map<AccountMember>(registerDto);

            // Store password as plain text (not recommended for production)
            account.MemberPassword = registerDto.MemberPassword;

            await _accountRepository.AddAsync(account);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto)
        {
            var account = await _accountRepository.FindAsync(a => a.EmailAddress == loginDto.EmailAddress && a.MemberPassword == loginDto.MemberPassword);
            var user = account.FirstOrDefault();

            if (user == null)
            {
                return null; // Authentication failed
            }

            return new LoginResponseDto
            {
                MemberId = user.MemberId,
                FullName = user.FullName,
                EmailAddress = user.EmailAddress,
                MemberRole = user.MemberRole,
                Token = "fake-jwt-token" // Replace with real JWT generation if needed
            };
        }

        public async Task DeleteAccountAsync(string memberId)
        {
            await _accountRepository.DeleteAsync(memberId);
        }
    }
}
