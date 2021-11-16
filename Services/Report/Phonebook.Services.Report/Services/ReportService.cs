using AutoMapper;
using MongoDB.Driver;
using Phonebook.Services.Report.Dtos;
using Phonebook.Services.Report.Settings;
using Phonebook.Shared.Dtos;
using Phonebook.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Services
{
    public class ReportService : IReportService
    {
        private readonly IMongoCollection<Models.Report> _reportCollection;
        private readonly IMapper _mapper;

        public ReportService(IMapper mapper, IDBSettings dbSettings)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var db = client.GetDatabase(dbSettings.DatabaseName);

            _reportCollection = db.GetCollection<Models.Report>(dbSettings.ReportCollectionName);
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<ReportDto>>> GetAllAsync()
        {
            var reports = await _reportCollection.Find(Report => true).ToListAsync();
            return ResponseDto<List<ReportDto>>.Success(_mapper.Map<List<ReportDto>>(reports), 200);
        }

        public async Task<ResponseDto<ReportDto>> CreateAsync()
        {
            var model = new Models.Report
            {
                Date = DateTime.Now,
                Status = ReportStatusEnum.Hazırlanıyor.ToString()
            };
            await _reportCollection.InsertOneAsync(model);

            var reportCreateResult = await _reportCollection.FindAsync(s => s.Date == model.Date);

            return ResponseDto<ReportDto>.Success(_mapper.Map<ReportDto>(reportCreateResult), 200);
        }


    }
}
