﻿using PRN222.ProductStore.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Services.Interfaces
{
    public interface IAccountMemberService
    {
        Task<AccountMember> GetAccountByEmail(string email, string password);
    }
}
