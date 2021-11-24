using MongoDB.Bson.Serialization.Attributes;
using Phonebook.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phonebook.Services.Report.Models
{
    public class ReportDetail : BaseEntity
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ReportID { get; set; }
        public string Location { get; set; }
        public int PersonCount { get; set; }
        public int GSMCount{ get; set; }
    }
}
