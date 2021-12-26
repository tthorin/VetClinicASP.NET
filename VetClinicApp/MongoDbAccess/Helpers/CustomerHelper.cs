// -----------------------------------------------------------------------------------------------
//  CustomerHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Helpers
{
    using Models;
    using MongoDB.Driver;
    using MongoDbAccess.Interfaces;

    internal static class CustomerHelper
    {
        private static readonly ICustomerCrud db = Factory.GetICustomerCrud();

        internal static async Task AddAnimalToCustomer(Animal animal)
        {
            var owner = await db.GetCustomerById(animal.OwnerId);
            if (owner.Pets == null) owner.Pets = new List<Animal>();
            if (!owner.Pets.Contains(animal)) owner.Pets.Add(animal);
            await db.UpdateCustomer(owner);
        }

        internal static async Task RemoveAnimalFromCustomer(Animal animalToBeDeleted)
        {
            var owner = await db.GetCustomerById(animalToBeDeleted.OwnerId);
            var animal = owner.Pets.Find(x => x.Id == animalToBeDeleted.Id);
            if (animal != null) owner.Pets.Remove(animal);
            await db.UpdateCustomer(owner);
        }

        internal static async Task UpdateAnimalOnCustomer(Animal dbAnimal)
        {
            var owner = await db.GetCustomerById(dbAnimal.OwnerId);
            var animalToUpdateIdx = owner.Pets.FindIndex(x => x.Id == dbAnimal.Id);
            owner.Pets[animalToUpdateIdx] = dbAnimal;
            await db.UpdateCustomer(owner);
        }

        internal static async Task DeleteAnimalsTogetherWithCustomer(string customerId)
        {
            var owner = await db.GetCustomerById(customerId);
            if (owner.Pets.Count > 0)
            {
                Database.MongoDbAccess mdb = new(Factory.GetConnectionStringHelper());
                foreach (var animal in owner.Pets)
                {
                    await mdb.AnimalCollection.DeleteOneAsync(x => x.Id == animal.Id);
                }
            }
        }
    }
}