using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace AracTakip.Models
{
    public class aracModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("tarih")]
        public string tarih { get; set; }
        [BsonElement("enlem")]

        public double enlem { get; set; }
        [BsonElement("boylam")]

        public double boylam { get; set; }
        [BsonElement("aracId")]

        public int aracId { get; set; }

    }
}