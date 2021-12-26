// -----------------------------------------------------------------------------------------------
//  IDBStats.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Database
{
    using System.Threading.Tasks;

    public interface IDBStats
    {
        Task<(int customersCount, int animalsCount)> GetDbStats();
    }
}