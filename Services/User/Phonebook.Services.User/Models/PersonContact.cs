using MongoDB.Bson.Serialization.Attributes;
using Phonebook.Shared;

namespace Phonebook.Services.User.Models
{
    public class PersonContact : BaseEntity
    {

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PersonID { get; set; }
        public string ContactType { get; set; }
        public string Description { get; set; }
    }
}