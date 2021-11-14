using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    internal class CreatePersonDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
    }
}
