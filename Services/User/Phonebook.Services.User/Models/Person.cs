using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Phonebook.Services.User.Models
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UUID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }

        [BsonIgnore]
        public ICollection<PersonContact> PersonContacts { get; set; }

    }
}
