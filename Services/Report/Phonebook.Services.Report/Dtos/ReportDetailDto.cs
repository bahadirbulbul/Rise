using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Dtos
{
    public class ReportDetailDto
    {
        public string ReportID { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int GSMCount { get; set; }
        public string Path { get; set; }
    }
}
