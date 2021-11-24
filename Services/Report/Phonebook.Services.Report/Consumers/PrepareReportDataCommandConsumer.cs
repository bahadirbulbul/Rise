using MassTransit;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Phonebook.Services.Report.Services;
using Phonebook.Shared;
using Phonebook.Shared.Enums;
using Phonebook.Shared.Messages;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Consumers
{
    public class PrepareReportDataCommandConsumer : IConsumer<PrepareReportDataCommand>
    {
        private readonly IReportService _reportService;
        public PrepareReportDataCommandConsumer(IReportService reportService)
        {
            _reportService = reportService;
        }

        //MQ'deki mesajı dinler.
        public async Task Consume(ConsumeContext<PrepareReportDataCommand> context)
        {
            //user microservice'ine gidip rapor datasını çeker.
            var client = new RestClient("http://localhost:5000");
            var request = new RestRequest("/services/user/PersonContact/GetReportData", Method.GET);

            var response = client.Execute<List<PrepareReportDataCommand>>(request).Data;

            //excel oluştur ve kaydet
            var excelPath = await CreateExcelFile(response);

            //dosya yolu için report tablosunda Path ve Durum sütunlarını günceller.
            var report = new Models.Report
            {
                Date = context.Message.Date,
                Status = Helper.GetDisplayName(ReportStatusEnum.Tamamlandı),
                Path = excelPath,
                UUID = context.Message.UUID
            };

            await _reportService.UpdateAsync(report);
        }

        public async Task<string> CreateExcelFile(List<PrepareReportDataCommand> dataList)
        {
            string sFileName = @"ExcelReports\" + Guid.NewGuid() + ".xlsx";

            var projectFolder = Directory.GetCurrentDirectory();
            FileInfo file = new FileInfo(Path.Combine(projectFolder, sFileName));
            var memory = new MemoryStream();

            using (var fs = new FileStream(Path.Combine(projectFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheet1");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Location");
                row.CreateCell(1).SetCellValue("Person Count");
                row.CreateCell(2).SetCellValue("GSM Count");
                foreach (var data in dataList)
                {
                    row = excelSheet.CreateRow(1);
                    row.CreateCell(0).SetCellValue(data.Location.ToString());
                    row.CreateCell(1).SetCellValue(data.PersonCount);
                    row.CreateCell(2).SetCellValue(data.GSMCount);
                }

                workbook.Write(fs);
            }

            using (var stream = new FileStream(Path.Combine(projectFolder, sFileName), FileMode.Open))
            {

                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return Path.Combine(projectFolder, sFileName).ToString();
        }
    }
}
