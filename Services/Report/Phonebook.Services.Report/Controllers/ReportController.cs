using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Services.Report.Services;
using Phonebook.Shared.ControllerBases;
using Phonebook.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public ReportController(IReportService reportService, ISendEndpointProvider sendEndpointProvider)
        {
            _reportService = reportService;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _reportService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //rapor oluştur.
            var createdReportDto = await _reportService.CreateAsync();

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:report-service"));
            var reportRequest = new PrepareReportDataCommand
            {
                UUID = createdReportDto.Data.UUID,
                Date = createdReportDto.Data.Date,
                Status = createdReportDto.Data.Status
            };
            await sendEndpoint.Send<PrepareReportDataCommand>(reportRequest);

            return CreateActionResultInstance(createdReportDto);
        }
    }
}
