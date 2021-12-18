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
            List<Task> tasks = new();
            var db = client.GetDatabase(MongoDbAccess.databaseName);
            var collection = db.GetCollection<Customer>(MongoDbAccess.ownerCollection);
            foreach (var customer in customerList.Take(count))
            {
                var filter = Builders<Customer>.Filter.Eq("Id", customer.Id);
                tasks.Add(collection.ReplaceOneAsync(filter, customer, new ReplaceOptions { IsUpsert = true }));
            }
            await Task.WhenAll(tasks);
        }

        private void MatchAndAddAnimalsToCustomers()
        {
            for (int i = 0; i < count; i++)
            {
                customerList[i].Pets.Add(animalList.First(x => x.OwnerId == customerList[i].Id));
            }
        }

        private async Task InsertAnimalsIntoDb()
        {
            var db = client.GetDatabase(MongoDbAccess.databaseName);
            var collection = db.GetCollection<Animal>(MongoDbAccess.animalCollection);
            await collection.InsertManyAsync(animalList.Take(count));
        }

        private void UpdateAnimalsWithOwnerId()
        {
            count = customerList.Count;
            if (animalList.Count < count) count = animalList.Count;
            for (int i = 0; i < count; i++)
            {
                animalList[i].OwnerId = customerList[i].Id;
            }
        }

        private async Task InsertCustomersIntoDb()
        {
            var db = client.GetDatabase(MongoDbAccess.databaseName);
            var collection = db.GetCollection<Customer>(MongoDbAccess.ownerCollection);
            await collection.InsertManyAsync(customerList);
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
            var db = client.GetDatabase(MongoDbAccess.databaseName);
            var collection = db.GetCollection<Customer>(MongoDbAccess.ownerCollection);
            return collection.EstimatedDocumentCount() == 0;
        }

        private bool CheckIfCustomerCollectionExists()
        {
            var db = client.GetDatabase(MongoDbAccess.databaseName);
            return db.ListCollectionNames().ToList().Contains(MongoDbAccess.ownerCollection);
        }

        private bool CheckIfDbExists()
        {
            var dbList = client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
            return dbList.Contains(MongoDbAccess.databaseName);
        }
    }
}
