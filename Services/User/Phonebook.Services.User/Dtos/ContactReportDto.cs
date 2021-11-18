using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    public class ContactReportDto
    {
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int GSMCount { get; set; }
    }
}
