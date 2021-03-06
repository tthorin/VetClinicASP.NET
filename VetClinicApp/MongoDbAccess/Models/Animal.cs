// -----------------------------------------------------------------------------------------------
//  Animal.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Models
{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;
    using System;

    public class Animal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Race { get; set; } = "";
        public string Gender { get; set; } = "";

        [BsonElement]
        [BsonDateTimeOptions(DateOnly=true, Kind = DateTimeKind.Local)]
        public DateTime Birthdate { get; set; } = new DateTime(2000, 1, 1);
        public string OwnerId { get; set; } = "";
    }
}
