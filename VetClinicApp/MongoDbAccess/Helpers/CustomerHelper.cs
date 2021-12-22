// -----------------------------------------------------------------------------------------------
//   by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Helpers
{
    using Database;
    using Models;
    using MongoDbAccess.Interfaces;
    using System;

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
            var animal = owner.Pets.Find(x=>x.Id == animalToBeDeleted.Id);
            if (animal!=null)owner.Pets.Remove(animal);
            await db.UpdateCustomer(owner);
        }

        internal static async Task UpdateAnimalOnCustomer(Animal dbAnimal)
        {
            var owner = await db.GetCustomerById(dbAnimal.OwnerId);
            var animalToUpdateIdx = owner.Pets.FindIndex(x=> x.Id==dbAnimal.Id);
            owner.Pets[animalToUpdateIdx] = dbAnimal;
            await db.UpdateCustomer(owner);
        }
    }
}