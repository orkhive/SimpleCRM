using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using SimpleCRM.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCRM.Data.DbContexts
{
    public class MembershipContext : DbContext
    {
        public DbSet<Member> Members { get; set; } = null!;

        public MembershipContext(DbContextOptions<MembershipContext> options) : base(options)
        {

        }
    }
}
