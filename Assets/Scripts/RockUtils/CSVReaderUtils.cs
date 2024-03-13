using UnityEngine;
using System.Linq;

namespace RockUtils {
    namespace CSVReaderUtils {
        public class CSVReaderUtils : MonoBehaviour {
            public static string[] ReadCSV(TextAsset textAssetData, int numDataColumns) {
                string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
                return data.Skip(numDataColumns).Take(data.Length).ToArray();
            }
        }
    }
}
