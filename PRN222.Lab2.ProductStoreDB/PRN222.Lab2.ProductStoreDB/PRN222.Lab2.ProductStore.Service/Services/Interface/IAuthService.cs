using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN222.Lab2.ProductStore.Repository.Models;

namespace PRN222.Lab2.ProductStore.Service.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string email, string password);
        void Logout();
        AccountMember GetCurrentUser();
    }
}
