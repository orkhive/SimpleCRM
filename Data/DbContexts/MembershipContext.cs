using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

using SimpleCRM.Data.Entities;
using SimpleCRM.Data.Interfaces;
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
