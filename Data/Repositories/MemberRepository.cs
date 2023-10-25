using Microsoft.EntityFrameworkCore;

using SimpleCRM.Common.Models;
using SimpleCRM.Common.Models.Member;
using SimpleCRM.Data.DbContexts;
using SimpleCRM.Data.Entities;
using SimpleCRM.Data.Interfaces;
using SimpleCRM.Data.Interfaces.Repositories;

namespace SimpleCRM.Data.Repositories
{
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        public MemberRepository(MembershipContext context) : base(context) { }

        public async Task<Member?> GetMemberAsync(Guid ID)
        {
            return await context.Members
                .Where(f => f.MemberID == ID)
                .FirstOrDefaultAsync();
        }

        public async Task<(List<Member>?, PaginationMetadata)> GetMembersAsync(MemberPagedQuery query)
        {
            var collection = context.Members.AsQueryable();

            //Apply Filters
            if (!String.IsNullOrEmpty(query.FirstName))
                collection = collection.Where(f => f.FirstName.Contains(query.FirstName));
            if (!String.IsNullOrEmpty(query.LastName))
                collection = collection.Where(f => f.LastName.Contains(query.LastName));

            //Apply Paging
            collection = query.ApplyPaging(collection);

            var TotalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(TotalItemCount, query.PageSize, query.PageNumber);
            var results = await collection.ToListAsync();
            return (results, paginationMetadata);

        }

    }
}
