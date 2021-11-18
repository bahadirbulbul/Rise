using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonebook.Shared.Messages
{
    public class PrepareReportDataCommand
    {
        public string UUID { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int GSMCount { get; set; }

    }
}
