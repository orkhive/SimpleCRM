using AutoMapper;
using SimpleCRM.Common.Models.Member;
using SimpleCRM.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleCRM.Logic.Profiles
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>().ReverseMap();
            CreateMap<CreateMemberDto, Member>().ReverseMap();
            CreateMap<UpdateMemberDto, Member>().ReverseMap();
            CreateMap<UpdateMemberDto, MemberDto>().ReverseMap();
        }
    }
}
