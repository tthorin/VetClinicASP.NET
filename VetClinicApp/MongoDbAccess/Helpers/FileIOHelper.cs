// -----------------------------------------------------------------------------------------------
//  FileIOHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Helpers
{
    using MongoDbAccess.Interfaces;
    using System.IO;

    public class FileIOHelper : IFileHelper
    {
        public string ReadAllFromFile(string file)
        {
            string output = "";
            if (File.Exists(file)) output = File.ReadAllText(file);
            return output;
        }
    }
}
