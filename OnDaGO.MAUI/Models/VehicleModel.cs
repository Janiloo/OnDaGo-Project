﻿// Models/VehicleModel.cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnDaGO.MAUI.Models
{
    public class VehicleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // This is the MongoDB document ID

        [BsonElement("currentLong")]
        public double CurrentLong { get; set; }

        [BsonElement("currentLat")]
        public double CurrentLat { get; set; }

        [BsonElement("puv_no")]
        public int PuvNo { get; set; }
    }
}
