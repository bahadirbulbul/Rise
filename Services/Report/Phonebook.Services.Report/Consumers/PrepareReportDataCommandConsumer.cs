using MassTransit;
using Phonebook.Services.Report.Services;
using Phonebook.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Consumers
{
    public class PrepareReportDataCommandConsumer : IConsumer<PrepareReportDataCommand>
    {
        private readonly IReportDetailService _reportDetailService;
        public PrepareReportDataCommandConsumer(IReportDetailService reportDetailService)
        {
            _reportDetailService = reportDetailService;
        }

        //MQ'deki mesajı dinler.
        public async Task Consume(ConsumeContext<PrepareReportDataCommand> context)
        {

            //user microservice'ine gidip rapor datasını çeker.
            //excel oluştur
            //report microservice'ine kaydet
            //rapor durumunu "tamamlandı" yap
            //dosya yolu için report tablosunda yeni bir sütun oluştur.

            //var data = 
                _reportDetailService.PrepareReportData();
            //context.Message.
        }
    }
}
