// -----------------------------------------------------------------------------------------------
//  PetOwner.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDbAccess.Interfaces;
    using System.Collections.Generic;

    public class Customer : IPetOwner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public List<Animal> Pets { get; set; }= new List<Animal>();
        public string PhoneNumber { get; set; } = "";
        [BsonIgnore]
        public string FullName { get => FirstName + " " + LastName; }
    }
}
