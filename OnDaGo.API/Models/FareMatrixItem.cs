﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace OnDaGo.API.Models
{
    public class FareMatrixItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId(); // Generate a new Id by default

        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Fare { get; set; }

        public decimal DiscountedFare { get; set; }
    }
    public class FareMatrixUpdateModel
    {
        public decimal? Fare { get; set; }
        public decimal? DiscountedFare { get; set; }
    }


}
