using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Dtos
{
    public class ReportDto
    {
        public string UUID { get; set; }

        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Path { get; set; }
    }
}
