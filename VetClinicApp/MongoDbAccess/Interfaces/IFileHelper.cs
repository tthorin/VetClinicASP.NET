// -----------------------------------------------------------------------------------------------
//  IFileIOHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Interfaces
{
    public interface IFileHelper
    {
        string ReadAllFromFile(string file);
    }
}