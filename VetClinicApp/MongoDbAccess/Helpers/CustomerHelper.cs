// -----------------------------------------------------------------------------------------------
//   by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Helpers
{
    using Database;
    using Models;

    internal static class CustomerHelper
    {
        static readonly MongoDbAccess db = Factory.GetDataAccess();
        internal static async Task AddAnimalToCustomer(Animal animal)
        {
            var owner = await db.GetCustomerById(animal.OwnerId);
            if (owner.Pets == null) owner.Pets = new List<Animal>();
            if (!owner.Pets.Contains(animal)) owner.Pets.Add(animal);
            await db.UpdateCustomer(owner);
        }

        internal static async Task RemoveAnimalFromCustomer(string animalId, string ownerId)
        {
            var owner = await db.GetCustomerById(ownerId);
            var animal = owner.Pets.Find(x=>x.Id == animalId);
            if (animal!=null)owner.Pets.Remove(animal);
            await db.UpdateCustomer(owner);
        }
    }
}