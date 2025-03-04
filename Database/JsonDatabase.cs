using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using NLog;
using SmartHealthTest.Utilities;

namespace SmartHealthTest.Database
{
    public class JsonDatabase<T>
    {
        private readonly string _filePath;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public JsonDatabase( string fileName )
        {
            _filePath = Path.Combine("SampleData", fileName);
        }

        public List<T> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<T>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        public void SaveAll(List<T> items)
        {
            try
            {
                logger.Info(MessageClass.UpdateDataMsg);
                string json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch(Exception ex)
            {
                logger.Error(MessageClass.UpdateDataErr, ex);
            }
        }
    }
}
