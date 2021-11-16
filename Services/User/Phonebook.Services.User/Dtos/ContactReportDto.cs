using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    public class ContactReportDto
    {
        public string Location { get; set; }
        public string PersonCount { get; set; }
        public string GSMCount { get; set; }
    }
}
