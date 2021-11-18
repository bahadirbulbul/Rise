using Phonebook.Services.Report.Dtos;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Services
{
    public interface IReportService
    {
        Task<ResponseDto<List<ReportDto>>> GetAllAsync();
        Task<ResponseDto<ReportDto>> CreateAsync();
        Task<ResponseDto<ReportDto>> UpdateAsync(Models.Report model);
    }
}
