using MongoDB.Bson.Serialization.Attributes;

namespace Phonebook.Services.User.Models
{
    public class PersonContact
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UUID { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PersonID { get; set; }
        public string ContactType { get; set; }
        public string Description { get; set; }
    }
}