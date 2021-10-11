using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Radix.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Domain.Entities
{
    public class SensorEvent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [JsonIgnore]
        public DateTime CreationDate { get; set; }

        public BsonDateTime Timestamp { get; set; }

        public string Tag { get; set; }

        public string Valor { get; set; }

        public bool Processed { get; set; }

        public void Validate()
        {
            if (Tag?.Split(".").Length < 3)
                throw new RadixException("Tag deve estar no formato pais.regiao.nomesensor. Exemplo: brasil.norte.sensor01");

            CreationDate = DateTime.Now;
            Processed = !string.IsNullOrWhiteSpace(Valor);
        }

    }
}
