using Newtonsoft.Json;
using System.IO;

namespace RockUtils {
    namespace JsonUtils {
        public class JsonUtils {
            public static void SaveData(object value, string path, Formatting formatting = Formatting.Indented) {
                string saveData = JsonConvert.SerializeObject(value, formatting);
                File.WriteAllText(path, saveData);
            }

            public static T LoadData<T>(string path) {
                string data = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(data);
            }
        }
    }
}

