using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyDatabase
{
    internal static class Serializer
    {
        public static void SaveAsJsonFormat<T>(T list, string fileName)
        {
            using (StreamWriter sw = File.CreateText(fileName))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(sw, list);
            }
        }
        public static T ReadAsJsonFormat<T>(string fileName)
        {
            T obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
            return obj;
        }
    }
}
