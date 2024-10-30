using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace spend_wise_api.models
{
    public class Registros
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateTime { get; set; }
        public string Descricao { get; set; }
        public string Identificador { get; set; }
        public float Valor { get; set; }
        public string? Categoria { get; set; }
    }
}
