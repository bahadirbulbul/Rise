using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    public class PersonContactDto
    {
       
        public string UUID { get; set; }
        public string PersonID { get; set; }
        public string ContactType { get; set; }
        public string Description { get; set; }
    }
}
