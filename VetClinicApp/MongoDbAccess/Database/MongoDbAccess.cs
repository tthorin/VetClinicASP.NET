// -----------------------------------------------------------------------------------------------
//  MongoDbAccess.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Database
{
    using global::MongoDbAccess.Interfaces;
    using Helpers;
    using Models;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MongoDbAccess
    {
        private IConnectionStringHelper csh;
        private readonly string connectionString = "";
        internal const string databaseName = "TT-VetClinic";
        internal const string ownerCollection = "owners";
        internal const string animalCollection = "animals";

        public MongoDbAccess(IConnectionStringHelper csh)
        {
            this.csh = csh;
            connectionString = csh.ConnectionString;
        }


        private IMongoCollection<T> MongoConnect<T>(in string collection)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);
            return db.GetCollection<T>(collection);
        }
        
        public async Task<Animal> GetAnimalById(string id)
        {
            var collection = MongoConnect<Animal>(animalCollection);
            var output = await collection.FindAsync(x => x.Id == id);
            return output.FirstOrDefault();
        }

        public Task CreateOwner(Customer owner)
        {
            var collection = MongoConnect<Customer>(ownerCollection);
            return collection.InsertOneAsync(owner);
        }
        public async Task<List<Customer>> GetAllCustomers()
        {
            var collection = MongoConnect<Customer>(ownerCollection);
            var result = await collection.FindAsync(_ => true);
            return result.ToList();
        }
        public async Task<List<Customer>> GetCustomersLastNameBeginsWith(string beginsWith)
        {
            var collection = MongoConnect<Customer>(ownerCollection);
            var result = await collection.FindAsync(x => x.LastName.ToLower().StartsWith(beginsWith));
            return result.ToList();
        }

        public async Task CreateAnimal(Animal animal)
        {
            var collection = MongoConnect<Animal>(animalCollection);
            await collection.InsertOneAsync(animal);
            if (animal.OwnerId != null) await CustomerHelper.AddAnimalToCustomer(animal);
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var collection = MongoConnect<Customer>(ownerCollection);
            var filter = Builders<Customer>.Filter.Eq("Id", id);
            var output = await collection.FindAsync(filter);
            return output.FirstOrDefault();
        }

        public async Task DeleteAnimalById(string animalId, string ownerId)
        {
            var collection = MongoConnect<Animal>(animalCollection);
            await collection.DeleteOneAsync(x => x.Id == animalId);
            await CustomerHelper.RemoveAnimalFromCustomer(animalId, ownerId);
        }

        public Task UpdateCustomer(Customer owner)
        {
            var collection = MongoConnect<Customer>(ownerCollection);
            var filter = Builders<Customer>.Filter.Eq("Id", owner.Id);
            return collection.ReplaceOneAsync(filter, owner, new ReplaceOptions { IsUpsert = true });
        }

        public Task DeleteCustomerById(string id)
        {
            var collection = MongoConnect<Customer>(ownerCollection);
            return collection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
