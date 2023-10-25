using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCRM.Common.Models.Member
{
    public class MemberPagedQuery : PagedQuery
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
