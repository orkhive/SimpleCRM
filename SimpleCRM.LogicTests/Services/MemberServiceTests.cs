using Microsoft.VisualStudio.TestTools.UnitTesting;

using AutoMapper;
using Moq;

using SimpleCRM.Data.Interfaces.Repositories;
using SimpleCRM.Common.Models.Member;
using SimpleCRM.Common.Models;
using SimpleCRM.Data.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SimpleCRM.Logic.Services.Tests
{
    [TestClass()]
    public class MemberServiceTests
    {
        private Mock<IMemberRepository> mockMemberRepository;
        private IMapper mapper;
        private MemberService memberService;

        [TestInitialize] 
        public void Init() 
        {
            mockMemberRepository = new Mock<IMemberRepository>();

            var config = new MapperConfiguration(cfg => { cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()); });
            mapper = new Mapper(config);

            memberService = new MemberService(mockMemberRepository.Object, mapper);
        }

        [TestMethod()]
        public void MemberServiceTest()
        {
            Assert.IsNotNull(memberService);
        }

        [TestMethod()]
        public async Task GetMemberAsyncTest()
        {
            //Arrange
            var ID = Guid.NewGuid();
            mockMemberRepository.Setup(f => f.GetMemberAsync(ID)).ReturnsAsync(new Data.Entities.Member() { MemberID = ID, FirstName = "Conner", LastName = "Test" });

            //Act
            var res = await memberService.GetMemberAsync(ID);

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(ID, res.MemberID);

        }

        [TestMethod()]
        public async Task GetMembersAsyncTest()
        {
            //Arrange
            var Query = new MemberPagedQuery()
            { 
                FirstName = "Conner",
                LastName = "Test",
                PageNumber = 1,
                PageSize = 5,
                OrderBy = "LastName"
            };
            mockMemberRepository.Setup(f=> f.GetMembersAsync(Query)).ReturnsAsync((
                new List<Member>() {
                    new Member() { MemberID = Guid.NewGuid(), FirstName = "Conner", LastName = "Test" },
                    new Member() { MemberID = Guid.NewGuid(), FirstName = "Conner", LastName = "Test 2" }
                }, 
                new PaginationMetadata(2, 5, 1))
            );

            //Act
            var res = await memberService.GetMembersAsync(Query);


            //Assert
            Assert.IsNotNull(res);
            Assert.IsNotNull(res.Item1);
            Assert.IsNotNull(res.Item2);
            Assert.AreEqual(2, res.Item1.Count());
        }

        [TestMethod()]
        public async Task CreateMemberAsyncTest()
        {
            //Arrange
            var createMember = new CreateMemberDto()
            {
                FirstName = "Conner",
                LastName = "Test"
            };
            mockMemberRepository.Setup(f => f.GetMembersAsync(It.IsAny<MemberPagedQuery>())).ReturnsAsync((new List<Member>(), new PaginationMetadata(0, 50, 1)));
            mockMemberRepository.Setup(f => f.AddAsync(It.IsAny<Member>()));
            mockMemberRepository.Setup(f => f.SaveChangesAsync()).ReturnsAsync(true);
            var modelState = new ModelStateDictionary();

            //Assert
            var res = await memberService.CreateMemberAsync(createMember, modelState);

            //Act
            Assert.IsNotNull(res);
            Assert.AreEqual(createMember.FirstName, res.FirstName);
            Assert.AreEqual(createMember.LastName, res.LastName);
            mockMemberRepository.Verify(f => f.GetMembersAsync(It.IsAny<MemberPagedQuery>()), Times.Once);
            mockMemberRepository.Verify(f => f.AddAsync(It.IsAny<Member>()), Times.Once);
            mockMemberRepository.Verify(f => f.SaveChangesAsync(), Times.Once);
            Assert.AreEqual(0, modelState.Count());
        }

        [TestMethod()]
        public async Task CreateMemberAsync_MemberDetailsExistTest()
        {
            //Arrange
            var createMember = new CreateMemberDto()
            {
                FirstName = "Conner",
                LastName = "Test"
            };
            mockMemberRepository.Setup(f => f.GetMembersAsync(It.IsAny<MemberPagedQuery>())).ReturnsAsync((new List<Member>() { new Member() { FirstName = createMember.FirstName,  LastName = createMember.LastName } }, new PaginationMetadata(1, 50, 1)));
            mockMemberRepository.Setup(f => f.AddAsync(It.IsAny<Member>()));
            mockMemberRepository.Setup(f => f.SaveChangesAsync()).ReturnsAsync(true);
            var modelState = new ModelStateDictionary();

            //Assert
            var res = await memberService.CreateMemberAsync(createMember, modelState);

            //Act
            Assert.IsNull(res);
            mockMemberRepository.Verify(f => f.GetMembersAsync(It.IsAny<MemberPagedQuery>()), Times.Once);
            mockMemberRepository.Verify(f => f.AddAsync(It.IsAny<Member>()), Times.Never);
            mockMemberRepository.Verify(f => f.SaveChangesAsync(), Times.Never);
            Assert.AreEqual(1, modelState.Count());
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            //Arrange
            var MemberID = Guid.NewGuid();
            var toUpdate = new UpdateMemberDto() { FirstName = "Connect", LastName = "Test" };
            mockMemberRepository.Setup(f => f.GetMemberAsync(MemberID)).ReturnsAsync(new Member() { MemberID = MemberID, FirstName = "", LastName = "" });
            mockMemberRepository.Setup(f => f.SaveChangesAsync()).ReturnsAsync(true);
            var modelState = new ModelStateDictionary();

            //Act
            var res = await memberService.UpdateAsync(MemberID, toUpdate, modelState);

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(toUpdate.FirstName, res.FirstName);
            Assert.AreEqual(toUpdate.LastName, res.LastName);
            mockMemberRepository.Verify(f => f.GetMemberAsync(MemberID), Times.Once);
            mockMemberRepository.Verify(f => f.SaveChangesAsync(), Times.Once);
            Assert.AreEqual(0, modelState.Count());
        }

        [TestMethod()]
        public async Task UpdateAsync_MemberDoesntExistTest()
        {
            //Arrange
            var MemberID = Guid.NewGuid();
            var toUpdate = new UpdateMemberDto() { FirstName = "Connect", LastName = "Test" };
            mockMemberRepository.Setup(f => f.GetMemberAsync(MemberID));
            mockMemberRepository.Setup(f => f.SaveChangesAsync()).ReturnsAsync(false);
            var modelState = new ModelStateDictionary();

            //Act
            var res = await memberService.UpdateAsync(MemberID, toUpdate, modelState);

            //Assert
            Assert.IsNull(res);
            mockMemberRepository.Verify(f => f.GetMemberAsync(MemberID), Times.Once);
            mockMemberRepository.Verify(f => f.SaveChangesAsync(), Times.Never);
            Assert.AreEqual(1, modelState.Count());
        }

        [TestMethod()]
        public async Task DeleteAsyncTest()
        {
            //Arrange
            var MemberID = Guid.NewGuid();
            mockMemberRepository.Setup(f => f.GetMemberAsync(MemberID)).ReturnsAsync(new Member() { MemberID = MemberID, FirstName = "", LastName = "" });
            mockMemberRepository.Setup(f => f.Delete(It.IsAny<Member>()));
            mockMemberRepository.Setup(f => f.SaveChangesAsync()).ReturnsAsync(true);
            var modelState = new ModelStateDictionary();

            //Act
            var res = await memberService.DeleteAsync(MemberID, modelState);

            //Assert
            Assert.IsTrue(res); 
            mockMemberRepository.Verify(f => f.GetMemberAsync(MemberID), Times.Once);
            mockMemberRepository.Verify(f => f.Delete(It.IsAny<Member>()), Times.Once);
            mockMemberRepository.Verify(f => f.SaveChangesAsync(), Times.Once);
            Assert.AreEqual(0, modelState.Count());
        }

        [TestMethod()]
        public async Task DeleteAsync_MemberDoesntExistTest()
        {
            //Arrange
            var MemberID = Guid.NewGuid();
            mockMemberRepository.Setup(f => f.GetMemberAsync(MemberID));
            mockMemberRepository.Setup(f => f.Delete(It.IsAny<Member>()));
            mockMemberRepository.Setup(f => f.SaveChangesAsync()).ReturnsAsync(false);
            var modelState = new ModelStateDictionary();

            //Act
            var res = await memberService.DeleteAsync(MemberID, modelState);

            //Assert
            Assert.IsFalse(res);
            mockMemberRepository.Verify(f => f.GetMemberAsync(MemberID), Times.Once);
            mockMemberRepository.Verify(f => f.Delete(It.IsAny<Member>()), Times.Never);
            mockMemberRepository.Verify(f => f.SaveChangesAsync(), Times.Never);
            Assert.AreEqual(1, modelState.Count());
        }
    }
}