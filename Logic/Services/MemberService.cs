using AutoMapper;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using SimpleCRM.Common.Interfaces.Services;
using SimpleCRM.Common.Models;
using SimpleCRM.Common.Models.Member;
using SimpleCRM.Data.Entities;
using SimpleCRM.Data.Interfaces.Repositories;

namespace SimpleCRM.Logic.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository memberRepository;
        private readonly IMapper mapper;

        public MemberService(IMemberRepository memberRepository, IMapper mapper)
        {
            this.memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<MemberDto?> GetMemberAsync(Guid id)
        {
            var member = await memberRepository.GetMemberAsync(id);
            if (member == null)
                return null;
            return mapper.Map<MemberDto>(member);
        }

        public async Task<(List<MemberDto>?, PaginationMetadata)> GetMembersAsync(MemberPagedQuery query)
        {
            var members = await memberRepository.GetMembersAsync(query);

            List<MemberDto>? membersToReturn = null;
            if (members.Item1 != null)
                membersToReturn = mapper.Map<List<MemberDto>>(members.Item1);

            return (membersToReturn, members.Item2);
        }

        public async Task<MemberDto?> CreateMemberAsync(CreateMemberDto create, ModelStateDictionary modelState)
        {
            //Firstly, Query For A Existing Member With The Details Provided
            var query = new MemberPagedQuery()
            {
                FirstName = create.FirstName,
                LastName = create.LastName
            };
            var dbMember = await memberRepository.GetMembersAsync(query);
            if (dbMember.Item1 != null && dbMember.Item1.Count() > 0)
            {
                modelState.AddModelError("LastName", $"Member With Name {create.FirstName} {create.LastName} Already Exists");
                return null;
            }

            //Then Create, Save To DB & Return
            var member = mapper.Map<Member>(create);
            await memberRepository.AddAsync(member);
            await memberRepository.SaveChangesAsync();
            return mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto?> UpdateAsync(Guid id, UpdateMemberDto update, ModelStateDictionary modelState)
        {
            var member = await memberRepository.GetMemberAsync(id);
            if (member == null)
            {
                modelState.AddModelError("id", $"Member Not Found");
                return null;
            }

            mapper.Map(update, member);
            await memberRepository.SaveChangesAsync();
            return mapper.Map<MemberDto>(member);
        }

        public async Task<bool> DeleteAsync(Guid id, ModelStateDictionary modelState)
        {
            var member = await memberRepository.GetMemberAsync(id);
            if (member == null)
            {
                modelState.AddModelError("id", $"Member Not Found");
                return false;
            }

            memberRepository.Delete(member);
            return await memberRepository.SaveChangesAsync();
        }

    }
}
