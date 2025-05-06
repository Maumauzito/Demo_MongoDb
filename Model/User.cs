using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace fiap_mongo.Model
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; } = string.Empty;

        [BsonElement("idade")]
        public int Idade { get; set; }
    }
}
