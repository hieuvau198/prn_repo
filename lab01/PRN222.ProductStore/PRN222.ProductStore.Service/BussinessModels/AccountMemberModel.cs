using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.BussinessModels
{
    public class AccountMemberModel
    {
        public string MemberId { get; set; }

        public string MemberPassword { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public int MemberRole { get; set; }
    }
}
