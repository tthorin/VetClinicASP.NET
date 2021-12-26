// -----------------------------------------------------------------------------------------------
//  StringHelper.cs by Thomas Thorin, Copyright (C) 2021.
//  Published under GNU General Public License v3 (GPL-3)
// -----------------------------------------------------------------------------------------------

namespace MongoDbAccess.Helpers
{
    internal static class StringHelper
    {
        internal static string Prettify(string stringToPrettify)
        {
            var output = stringToPrettify.Trim();
            if (output.Length >= 2) output = output[..1].ToUpper() + output[1..].ToLower();
            return output;
        }
    }
}
