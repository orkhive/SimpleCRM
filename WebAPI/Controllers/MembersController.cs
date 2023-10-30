using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using SimpleCRM.Common.Interfaces.Services;
using SimpleCRM.Common.Models.Member;

namespace SimpleCRM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService memberService;
        private readonly IMapper mapper;

        public MembersController(IMemberService memberService, IMapper mapper)
        {
            this.memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Returns a OK with the member (if found), or will return Not Found if no member is found
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        [HttpGet("{MemberID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> GetMember(Guid MemberID)
        {
            var member = await memberService.GetMemberAsync(MemberID);
            if (member == null)
                return NotFound();
            return Ok(member);
        }

        /// <summary>
        /// Returns a OK with the list of members matching the query params provided (even if there are none).
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<MemberDto>>> GetMembers([FromQuery] MemberPagedQuery Query)
        {
            var (members, paginationMetadata) = await memberService.GetMembersAsync(Query);
            if (members == null)
                return BadRequest();
            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata));
            return Ok(members);
        }

        /// <summary>
        /// Creates the member and commits to the database. Returns the new member as created if successful.
        /// </summary>
        /// <param name="CreateMember"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberDto CreateMember)
        {
            var member = await memberService.CreateMemberAsync(CreateMember, ModelState);
            if (member == null)
                return BadRequest();
            return Ok(member);
        }

        /// <summary>
        /// Updates the member in full with the information provided.
        /// Returns a OK with the full member if successful.
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="UpdateMember"></param>
        /// <returns></returns>
        [HttpPut("{MemberID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> UpdateMember(Guid MemberID, UpdateMemberDto UpdateMember)
        {
            var member = await memberService.GetMemberAsync(MemberID);
            if (member == null)
                return NotFound();

            member = await memberService.UpdateAsync(MemberID, UpdateMember, ModelState);
            if (member == null)
                return BadRequest();
            return Ok(member);
        }

        /// <summary>
        /// Updates The Member With The Information Provided.
        /// Returns a ok with the full member if successful.
        /// </summary>
        /// <param name="MemberID"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{MemberID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberDto>> PatchMember(Guid MemberID, JsonPatchDocument<UpdateMemberDto> patchDocument)
        {
            if ((patchDocument.Operations?.Count() ?? 0) == 0)
                return BadRequest();

            var member = await memberService.GetMemberAsync(MemberID);
            if (member == null)
                return NotFound();

            //Preform The Update
            var toPatch = mapper.Map<UpdateMemberDto>(member);
            patchDocument.ApplyTo(toPatch, ModelState); //Apply The Patch Document, And If there Are Any Errors Return Then To The User

            if (!ModelState.IsValid) //Only Validates The Patch Model, Not The Target Object
                return BadRequest(ModelState);
            if (!TryValidateModel(toPatch)) //So validate the target object
                return BadRequest(ModelState);

            member = await memberService.UpdateAsync(MemberID, toPatch, ModelState);
            if (member == null)
                return BadRequest();
            return Ok(member);
        }

        /// <summary>
        /// Deletes the member from the database if found. Returns NoContent if successful.
        /// </summary>
        /// <param name="MemberID"></param>
        /// <returns></returns>
        [HttpDelete("{MemberID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMember(Guid MemberID)
        {
            var member = await memberService.GetMemberAsync(MemberID);
            if (member == null)
                return NotFound();

            var result = await memberService.DeleteAsync(MemberID, ModelState);
            if (!result)
                return BadRequest();
            return NoContent();
        }
    }
}
