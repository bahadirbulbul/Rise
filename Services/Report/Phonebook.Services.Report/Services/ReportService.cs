using AutoMapper;
using Phonebook.Services.Report.Dtos;
using Phonebook.Shared;
using Phonebook.Shared.Dtos;
using Phonebook.Shared.Enums;
using Phonebook.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Services
{
    public class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Models.Report> _repository;

        public ReportService(IRepository<Models.Report> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<ReportDto>>> GetAllAsync()
        {
            var reports = await _repository.GetListWithFiltersAsync(x => true);

            return ResponseDto<List<ReportDto>>.Success(_mapper.Map<List<Models.Report>, List<ReportDto>>(reports), 200);
        }

        public async Task<ResponseDto<ReportDto>> CreateAsync()
        {
            var model = new Models.Report
            {
                Date = DateTime.Now,
                Status = Helper.GetDisplayName(ReportStatusEnum.Hazırlanıyor)
            };
            await _repository.AddAsync(model);

            return ResponseDto<ReportDto>.Success(_mapper.Map<ReportDto>(model), 200);
        }

        public async Task<ResponseDto<ReportDto>> UpdateAsync(Models.Report model)
        {
            var reportCreateResult = await _repository.UpdateAsync(model.UUID, model);

            return ResponseDto<ReportDto>.Success(_mapper.Map<ReportDto>(reportCreateResult), 200);
        }

    }
}
