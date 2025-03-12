using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.DTOs
{
    public class LoginRequestDto
    {
        public string EmailAddress { get; set; }
        public string MemberPassword { get; set; }
    }
}
