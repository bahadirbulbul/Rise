using AutoMapper;
using Moq;
using Phonebook.Services.Report.Services;
using Phonebook.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Phonebook.Services.Report.Test
{
    public class ReportTests
    {
        private readonly Mock<IRepository<Models.Report>> _mockRepository;
        public ReportTests()
        {
            _mockRepository = new Mock<IRepository<Models.Report>>();
        }
        [Fact]
        public async Task GetAllAsync_ExistRecords_ReturnRecords()
        {
            //arrange
            _mockRepository.Setup(x => x.GetListWithFiltersAsync(s => true)).Returns(Task.FromResult(new List<Models.Report>
            {
                new Models.Report
                {
                    Date=DateTime.Now,
                    Path="ahmet",
                    Status="Hazırlanıyor",
                    UUID="asdasd123123"
                }
            }));
            var service = new ReportService(_mockRepository.Object, new Mock<IMapper>().Object);

            //act
            var returnedValue = await service.GetAllAsync();

            //assert
            Assert.True(1 == returnedValue.Data.Count());
            Assert.True(returnedValue.Data.First().Path == "emrah");
        }


        //controller testleri yazılmayacak
    }
}
