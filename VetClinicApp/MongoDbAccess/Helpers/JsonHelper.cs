﻿namespace MongoDbAccess.Helpers
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using MongoDbAccess.Interfaces;

    public class JsonHelper : IJsonHelper
    {
        public List<T> DeserializeList<T>(string json)
        {
            List<T> output = new();
            output = JsonConvert.DeserializeObject<List<T>>(json);
            return output;
        }
    }
        
}
