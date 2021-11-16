using AutoMapper;
using MongoDB.Driver;
using Phonebook.Services.Report.Dtos;
using Phonebook.Services.Report.Settings;
using Phonebook.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Services
{
    public class ReportDetailService : IReportDetailService
    {
        private readonly IMongoCollection<Models.ReportDetail> _reportDetailCollection;
        private readonly IMapper _mapper;

        public ReportDetailService(IMapper mapper, IDBSettings dbSettings)
        {
            var client = new MongoClient(dbSettings.ConnectionString);
            var db = client.GetDatabase(dbSettings.DatabaseName);

            _reportDetailCollection = db.GetCollection<Models.ReportDetail>(dbSettings.ReportDetailCollectionName);
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<ReportDetailDto>>> GetDetailsByReportId(string uuid)
        {
            var reportDetails = await _reportDetailCollection.Find(s=>s.ReportID==uuid).ToListAsync();
            return ResponseDto<List<ReportDetailDto>>.Success(_mapper.Map<List<ReportDetailDto>>(reportDetails), 200);
        }
    }
}
