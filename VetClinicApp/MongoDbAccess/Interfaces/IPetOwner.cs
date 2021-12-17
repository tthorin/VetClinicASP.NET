// -----------------------------------------------------------------------------------------------
//  IPetOwner.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    using MongoDbAccess.Models;
    using System.Collections.Generic;

    public interface IPetOwner
    {
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        List<Animal> Pets { get; set; }
    }
}