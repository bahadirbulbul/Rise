﻿using Phonebook.Services.Report.Dtos;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Services
{
    public interface IReportDetailService
    {
        Task<ResponseDto<List<ReportDetailDto>>> GetDetailsByReportId(string uuid);
        void PrepareReportData();
        Task<ResponseDto<NoContent>> CreateAsync(string reportUUID);
    }
}
