using AutoMapper;
using Moq;
using Phonebook.Services.Report.Dtos;
using Phonebook.Services.Report.Services;
using Phonebook.Shared;
using Phonebook.Shared.Enums;
using Phonebook.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Phonebook.Services.Report.Test.ServicesTests
{
    public class ReportServiceTests
    {
        private readonly Mock<IRepository<Models.Report>> _mockRepository;
        private readonly IMapper _mapper;
        private readonly ReportService _reportService;


        private List<Models.Report> reports;
        private Models.Report report1;
        private Models.Report report2;

        public ReportServiceTests()
        {
            _mockRepository = new Mock<IRepository<Models.Report>>();
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new Mappings.Mapping());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _reportService = new ReportService(_mockRepository.Object, _mapper);

            #region Test edilecek nesne ve listeler
            report1 = new Models.Report
            {
                Date = DateTime.Now,
                Path = "ExcelReports/1-report.xlsx",
                Status = "Hazırlanıyor",
                UUID = "asdasd123123"
            };
            report2 = new Models.Report
            {
                Date = DateTime.Now,
                Path = "ExcelReports/2-report.xlsx",
                Status = "Tamamlandı",
                UUID = "22333345"
            };
            reports = new List<Models.Report> { report1, report2 };
            #endregion
        }

        [Fact]
        public async Task GetAllAsync_ExistRecords_ReturnRecords()
        {
            _mockRepository.Setup(x => x.GetListWithFiltersAsync(s => true)).Returns(Task.FromResult(reports));

            var returnedValue = await _reportService.GetAllAsync();

            Assert.True(2 == returnedValue.Data.Count());
            Assert.Contains("ExcelReports", returnedValue.Data.First().Path);
            Assert.IsType<DateTime>(returnedValue.Data.First().Date);
            Assert.Equal(200, returnedValue.StatusCode);
        }

        [Fact]
        public async Task CreateAsync_InsertRecord_ReturnInsertedRecord()
        {
            _mockRepository.Setup(s => s.AddAsync(It.IsAny<Models.Report>())).Returns(Task.FromResult(report1));

            var actual = await _reportService.CreateAsync();

            Assert.NotNull(actual.Data);
            Assert.Null(actual.Data.UUID);
            Assert.Null(actual.Data.Path);
            Assert.Equal(Helper.GetDisplayName(ReportStatusEnum.Hazırlanıyor), actual.Data.Status);
            Assert.IsType<DateTime>(actual.Data.Date);
            Assert.Equal(200, actual.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_UpdateRecord_ReturnUpdatedRecord()
        {
            _mockRepository.Setup(s => s.UpdateAsync(report2.UUID, report2)).Returns(Task.FromResult(report2));

            var actual = await _reportService.UpdateAsync(report2);

            Assert.NotNull(actual.Data);
            Assert.Contains("ExcelReports", actual.Data.Path);
            Assert.IsType<DateTime>(actual.Data.Date);
            Assert.Equal(200, actual.StatusCode);
        }

    }
}
