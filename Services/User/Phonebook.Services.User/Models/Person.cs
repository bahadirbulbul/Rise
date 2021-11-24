using MongoDB.Bson.Serialization.Attributes;
using Phonebook.Shared;
using System.Collections.Generic;

namespace Phonebook.Services.User.Models
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }

        [BsonIgnore]
        public ICollection<PersonContact> PersonContacts { get; set; }

    }
}
