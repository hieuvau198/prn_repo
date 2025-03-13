using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PRN222.Lab2.ProductStore.Repository.Models;
using PRN222.Lab2.ProductStore.Repository.Repositories.Interface;
using PRN222.Lab2.ProductStore.Service.Services.Interface;

namespace PRN222.Lab2.ProductStore.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SessionKeyUser = "CurrentUser";

        public AuthService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.AccountMembers
                .GetAsync(u => u.EmailAddress == email && u.MemberPassword == password);

            if (!user.Any())
            {
                return false; // Đăng nhập thất bại
            }

            var account = user.First();

            // Lưu thông tin user vào Session
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(SessionKeyUser, JsonSerializer.Serialize(account));

            return true; // Đăng nhập thành công
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove(SessionKeyUser);
        }

        public AccountMember GetCurrentUser()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var userData = session.GetString(SessionKeyUser);

            return userData != null ? JsonSerializer.Deserialize<AccountMember>(userData) : null;
        }
    }
}