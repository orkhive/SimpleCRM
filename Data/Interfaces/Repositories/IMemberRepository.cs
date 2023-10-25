using SimpleCRM.Common.Models;
using SimpleCRM.Common.Models.Member;
using SimpleCRM.Data.Entities;

namespace SimpleCRM.Data.Interfaces.Repositories
{
    public interface IMemberRepository : IBaseRepository<Member>
    {
        Task<Member?> GetMemberAsync(Guid ID);
        Task<(List<Member>?, PaginationMetadata)> GetMembersAsync(MemberPagedQuery query);
    }
}