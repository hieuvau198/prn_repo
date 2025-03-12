using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Interfaces
{
    public interface IAccountMemberService
    {
        Task<IEnumerable<AccountMemberDto>> GetAllAccountsAsync();
        Task<AccountMemberDto> GetAccountByIdAsync(string memberId);
        Task<AccountMemberDto> GetAccountByEmailAsync(string email);
        Task RegisterAccountAsync(RegisterAccountDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
        Task DeleteAccountAsync(string memberId);
    }
}
