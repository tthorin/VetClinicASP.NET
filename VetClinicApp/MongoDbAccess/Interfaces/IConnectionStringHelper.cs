// -----------------------------------------------------------------------------------------------
//  IConnectionStringHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    public interface IConnectionStringHelper
    {
        string ConnectionString { get; set; }
    }
}