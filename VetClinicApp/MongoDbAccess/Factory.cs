// -----------------------------------------------------------------------------------------------
//  Factory.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess
{
    using MongoDbAccess.Database;
    using MongoDbAccess.Models;
    using MongoDbAccess.Helpers;
    using MongoDbAccess.Interfaces;

    public static class Factory
    {
        public static Customer GetCustomer()
        {
            return new Customer();
        }

        public static MongoDbAccess GetDataAccess()
        {
            return new MongoDbAccess(GetConnectionStringHelper());
        }
        internal static IFileHelper GetFileHelper()
        {
            return new FileIOHelper();
        }
        internal static IJsonHelper GetJsonHelper()
        {
            return new JsonHelper();
        }
        //todo: change back to internal?
        public static DbSeeder GetDbSeeder()
        {
            return new DbSeeder(GetConnectionStringHelper(), GetFileHelper(), GetJsonHelper());
        }
        internal static IConnectionStringHelper GetConnectionStringHelper()
        {
            
            return ConnectionStringHelper.Instance;
        }
    }
}
