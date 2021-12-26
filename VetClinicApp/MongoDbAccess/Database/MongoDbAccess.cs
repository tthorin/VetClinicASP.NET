// -----------------------------------------------------------------------------------------------
//  MongoDbAccess.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Database
{
    using Interfaces;
    using Helpers;
    using Models;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;

    public class MongoDbAccess : ICustomerAnimalCrud, IDBStats
    {
        private readonly string connectionString;
        internal const string DatabaseName = "TT-VetClinic";
        internal const string OwnerCollection = "owners";
        internal const string PetCollection = "animals";
        public IMongoCollection<Customer> CustomerCollection { get => MongoConnect<Customer>(OwnerCollection); }
        public IMongoCollection<Animal> AnimalCollection { get => MongoConnect<Animal>(PetCollection); }

        public MongoDbAccess(IConnectionStringHelper csh)
        {
            connectionString = csh.ConnectionString;
        }

        private IMongoCollection<T> MongoConnect<T>(in string collection)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }

        public async Task<(int customersCount, int animalsCount)> GetDbStats()
        {
            var customerCount = await CustomerCollection.EstimatedDocumentCountAsync();
            var animalCount = await AnimalCollection.EstimatedDocumentCountAsync();
            return ((int)customerCount, (int)animalCount);
        }

        public async Task<Animal> GetAnimalById(string id)
        {
            var output = await AnimalCollection.FindAsync(x => x.Id == id);
            return output.FirstOrDefault();
        }

        public async Task<List<Animal>> GetAnimalsByNameBeginsWith(string searchString)
        {
            var result = await AnimalCollection.FindAsync(x =>
                x.Name.ToLower().StartsWith(searchString.ToLower()));
            return result.ToList();
        }

        public async Task<List<Animal>> GetAllAnimal()
        {
            var result = await AnimalCollection.FindAsync(_ => true);
            return result.ToList();
        }

        public Task CreateCustomer(Customer owner)
        {
            return CustomerCollection.InsertOneAsync(owner);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var result = await CustomerCollection.FindAsync(_ => true);
            return result.ToList();
        }

        public async Task<List<Customer>> GetCustomersEitherNameBeginsWith(string beginsWith)
        {
            var result = await CustomerCollection.FindAsync(x =>
                x.LastName.ToLower().StartsWith(beginsWith.ToLower()) ||
                x.FirstName.ToLower().StartsWith(beginsWith.ToLower()));
            return result.ToList();
        }

        public async Task CreateAnimal(Animal animal)
        {
            await AnimalCollection.InsertOneAsync(animal);
            await CustomerHelper.AddAnimalToCustomer(animal);
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var filter = Builders<Customer>.Filter.Eq("Id", id);
            var output = await CustomerCollection.FindAsync(filter);
            return output.FirstOrDefault();
        }

        public async Task<bool> UpdateAnimal(Animal animal)
        {
            var filter = Builders<Animal>.Filter.Eq("Id", animal.Id);
            var result = await AnimalCollection.ReplaceOneAsync(filter, animal, new ReplaceOptions { IsUpsert = true });
            if (result.IsModifiedCountAvailable && result.ModifiedCount > 0)
            {
                await CustomerHelper.UpdateAnimalOnCustomer(animal);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAnimal(Animal animal)
        {
            var result = await AnimalCollection.DeleteOneAsync(x => x.Id == animal.Id);
            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                await CustomerHelper.RemoveAnimalFromCustomer(animal);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCustomer(Customer owner)
        {
            var filter = Builders<Customer>.Filter.Eq("Id", owner.Id);
            var result = await CustomerCollection.ReplaceOneAsync(filter, owner, new ReplaceOptions { IsUpsert = true });
            return result.IsModifiedCountAvailable && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCustomerById(string id)
        {
            await CustomerHelper.DeleteAnimalsTogetherWithCustomer(id);
            var result = await CustomerCollection.DeleteOneAsync(x => x.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}