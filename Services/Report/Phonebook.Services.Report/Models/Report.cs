using MongoDB.Bson.Serialization.Attributes;
using Phonebook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Models
{
    public class Report : BaseEntity
    {

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Path { get; set; }
    }
}
