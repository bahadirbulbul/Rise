using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Services.Report.Services;
using Phonebook.Shared.ControllerBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDetailController : BaseController
    {
        private readonly IReportDetailService _reportDetailService;

        public ReportDetailController(IReportDetailService reportDetailService)
        {
            _reportDetailService = reportDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(string uuid)
        {
            return CreateActionResultInstance(await _reportDetailService.GetDetailsByReportId(uuid));
        }
    }
}
