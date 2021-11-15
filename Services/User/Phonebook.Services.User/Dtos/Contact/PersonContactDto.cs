﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.User.Dtos
{
    internal class PersonContactDto
    {
       
        public string UUID { get; set; }
        public string PersonId { get; set; }
        public string ContactType { get; set; }
        public string Description { get; set; }
    }
}
