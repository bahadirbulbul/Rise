using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Services.Report.Services;
using Phonebook.Shared.ControllerBases;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportDetailController : BaseController
    {
        private readonly IReportDetailService _reportDetailService;

        public ReportDetailController(IReportDetailService reportDetailService)
        {
            _reportDetailService = reportDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails()
        {
            _reportDetailService.PrepareReportData();
            return CreateActionResultInstance(new ResponseDto<bool>());
        }
    }
}
