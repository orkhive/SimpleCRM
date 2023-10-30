
using Microsoft.AspNetCore.Mvc.ModelBinding;

using SimpleCRM.Common.Models;
using SimpleCRM.Common.Models.Member;


namespace SimpleCRM.Common.Interfaces.Services
{
    public interface IMemberService
    {
        Task<MemberDto?> CreateMemberAsync(CreateMemberDto create, ModelStateDictionary modelState);
        Task<bool> DeleteAsync(Guid id, ModelStateDictionary modelState);
        Task<MemberDto?> GetMemberAsync(Guid id);
        Task<(List<MemberDto>?, PaginationMetadata)> GetMembersAsync(MemberPagedQuery query);
        Task<MemberDto?> UpdateAsync(Guid id, UpdateMemberDto update, ModelStateDictionary modelState);
    }
}