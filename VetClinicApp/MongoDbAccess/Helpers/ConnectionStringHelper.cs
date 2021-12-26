// -----------------------------------------------------------------------------------------------
//  ConnectionStringHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Helpers
{
    using MongoDbAccess.Interfaces;
    using System;

    internal sealed class ConnectionStringHelper : IConnectionStringHelper
    {
        private static ConnectionStringHelper? _instance = null;

        private static readonly object padlock = new();
        public string ConnectionString { get; set; } = "";
        public static ConnectionStringHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null) _instance = new ConnectionStringHelper();
                }
                return _instance;
            }
        }
        private ConnectionStringHelper()
        {
            ReadConnectionString();
        }

        private void ReadConnectionString()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var file = @"ConnectionStrings\MongoDb.txt";
            var combined = Path.Combine(documents, file);
            if (File.Exists(combined)) ConnectionString = File.ReadAllText(combined);
            else ConnectionString = @"mongodb://localhost:27017";
        }

    }
}
