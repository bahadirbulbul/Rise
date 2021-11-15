using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    internal class EditPersonContactDto
    {
        public string ID { get; set; }
        public string ContactType { get; set; }
        public string Description { get; set; }
    }
}
