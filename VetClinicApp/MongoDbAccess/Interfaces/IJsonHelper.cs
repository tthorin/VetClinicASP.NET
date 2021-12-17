// -----------------------------------------------------------------------------------------------
//  IJsonHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    using System.Collections.Generic;

    public interface IJsonHelper
    {
        List<T> DeserializeList<T>(string json);
    }
}
