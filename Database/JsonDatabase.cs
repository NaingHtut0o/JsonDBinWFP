using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace SmartHealthTest.Database
{
    public class JsonDatabase<T>
    {
        private readonly string _filePath;

        public JsonDatabase( string fileName )
        {
            _filePath = Path.Combine("SampleData", fileName);
        }

        public List<T> GetAll()
        {
            if(!File.Exists(_filePath)) return new List<T>();

            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        public void SaveAll(List<T> items)
        {
            string json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
