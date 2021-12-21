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

    public class MongoDbAccess
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

        public async Task<(int customers, int animals)> GetDbStats()
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

        public Task CreateOwner(Customer owner)
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

        public async Task UpdateAnimal(Animal animal)
        {
            var filter = Builders<Animal>.Filter.Eq("Id", animal.Id);
            await AnimalCollection.ReplaceOneAsync(filter, animal, new ReplaceOptions { IsUpsert = true });
            await CustomerHelper.UpdateAnimalOnCustomer(animal);
        }

        public async Task DeleteAnimal(Animal animal)
        {
            await AnimalCollection.DeleteOneAsync(x => x.Id == animal.Id);
            await CustomerHelper.RemoveAnimalFromCustomer(animal);
        }

        public Task UpdateCustomer(Customer owner)
        {
            var filter = Builders<Customer>.Filter.Eq("Id", owner.Id);
            return CustomerCollection.ReplaceOneAsync(filter, owner, new ReplaceOptions {IsUpsert = true});
        }

        public Task DeleteCustomerById(string id)
        {
            return CustomerCollection.DeleteOneAsync(x => x.Id == id);
        }

        public List<Customer> GetCustomersWithoutAnimals()
        {
            var filter = Builders<Customer>.Filter.Eq("Pets.Count",0);
            var output = CustomerCollection.AsQueryable().Where(x => x.Pets.Count == 0).ToList();
            return output.ToList();
        }
    }
}