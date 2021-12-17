// -----------------------------------------------------------------------------------------------
//   by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    using MongoDbAccess.Models;

    public interface IAnimal
    {
        public string Id { get; set; }
        public string  Name { get; set; }
        public DateTime Birthdate { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
        public string OwnerId { get; set; }
    }
}