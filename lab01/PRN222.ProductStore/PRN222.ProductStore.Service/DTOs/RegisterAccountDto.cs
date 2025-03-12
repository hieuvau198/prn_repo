using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.DTOs
{
    public class RegisterAccountDto
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string MemberPassword { get; set; }
        public int MemberRole { get; set; }
    }
}
