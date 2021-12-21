// -----------------------------------------------------------------------------------------------
//  DbSeeder.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Database
{
    using Models;
    using Interfaces;
    using MongoDB.Driver;
    using System;
    using static Factory;
    using System.Collections.Generic;

    public class DbSeeder
    {
        private readonly IFileHelper fileHelper;
        private readonly IJsonHelper jsonHelper;
        private readonly IMongoClient client;
        private List<Customer> customerList = new();
        private List<Animal> animalList = new();
        private int count;

        public DbSeeder(IConnectionStringHelper csh, IFileHelper fh, IJsonHelper jh)
        {
            this.fileHelper = fh;
            this.jsonHelper = jh;
            client = new MongoClient(csh.ConnectionString);
        }
        public async Task SeedDB()
        {
            if (!CheckIfDbExists() || !CheckIfCustomerCollectionExists() || IsCustomerCollectionEmpty())
            {
                DoImports();
                await InsertCustomersIntoDb();
                UpdateAnimalsWithOwnerId();
                await InsertAnimalsIntoDb();
                MatchAndAddAnimalsToCustomers();
                await UpdateCustomersInDb();
            }
        }

        private async Task UpdateCustomersInDb()
        {   
            var db = client.GetDatabase(MongoDbAccess.DatabaseName);
            var collection = db.GetCollection<Customer>(MongoDbAccess.OwnerCollection);
            var bulkReplace = new List<WriteModel<Customer>>();
            foreach (var customer in customerList.Take(count))
            {
                var filter = Builders<Customer>.Filter.Eq("Id", customer.Id);
                bulkReplace.Add(new ReplaceOneModel<Customer>(filter, customer) { IsUpsert = true });
            }
            await collection.BulkWriteAsync(bulkReplace);
        }

        private void MatchAndAddAnimalsToCustomers()
        {
            for (int i = 0; i < count; i++)
            {
                customerList[i].Pets.Add(animalList.First(x => x.OwnerId == customerList[i].Id));
            }
            var noPet = customerList.Where(x => x.Pets.Count == 0).ToList();
        }

        private async Task InsertAnimalsIntoDb()
        {
            var db = client.GetDatabase(MongoDbAccess.DatabaseName);
            var collection = db.GetCollection<Animal>(MongoDbAccess.PetCollection);
            await collection.InsertManyAsync(animalList.Take(count));
            var noId = animalList.Where(x => string.IsNullOrEmpty(x.Id)).ToList();
        }

        private void UpdateAnimalsWithOwnerId()
        {
            count = customerList.Count;
            if (animalList.Count < count) count = animalList.Count;
            for (int i = 0; i < count; i++)
            {
                animalList[i].OwnerId = customerList[i].Id;
            }
            var noOwner = animalList.Where(x => string.IsNullOrEmpty(x.OwnerId)).ToList();
        }

        private async Task InsertCustomersIntoDb()
        {
            var db = client.GetDatabase(MongoDbAccess.DatabaseName);
            var collection = db.GetCollection<Customer>(MongoDbAccess.OwnerCollection);
            await collection.InsertManyAsync(customerList);
            var noId = customerList.Where(x => string.IsNullOrEmpty(x.Id)).ToList();
        }

        private void DoImports()
        {
            customerList = GetCustomers();
            animalList = GetAnimals();
        }

        private List<Animal> GetAnimals()
        {
            var jsonAnimals = ImportAnimalsFile();
            return jsonHelper.DeserializeList<Animal>(jsonAnimals);
        }

        private string ImportAnimalsFile()
        {
            const string file = @"json\animals.json";
            return fileHelper.ReadAllFromFile(file);
        }

        private List<Customer> GetCustomers()
        {
            var jsonCustomers = ImportCustomersFile();
            return jsonHelper.DeserializeList<Customer>(jsonCustomers);
        }

        private string ImportCustomersFile()
        {
            const string file = @"json\customers.json";
            return fileHelper.ReadAllFromFile(file);
        }

        private bool IsCustomerCollectionEmpty()
        {
            var db = client.GetDatabase(MongoDbAccess.DatabaseName);
            var collection = db.GetCollection<Customer>(MongoDbAccess.OwnerCollection);
            return collection.EstimatedDocumentCount() == 0;
        }

        private bool CheckIfCustomerCollectionExists()
        {
            var db = client.GetDatabase(MongoDbAccess.DatabaseName);
            return db.ListCollectionNames().ToList().Contains(MongoDbAccess.OwnerCollection);
        }

        private bool CheckIfDbExists()
        {
            var dbList = client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
            return dbList.Contains(MongoDbAccess.DatabaseName);
        }
    }
}
