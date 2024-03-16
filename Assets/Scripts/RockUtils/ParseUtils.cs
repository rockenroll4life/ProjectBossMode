using UnityEngine;

namespace RockUtils {
    namespace ParseUtils {
        public class ParseUtils {
            public static T Parse<T>(string value) {
                if (typeof(T).IsEnum) {
                    return (T) System.Enum.Parse(typeof(T), value);
                } else if (typeof(T) == typeof(int)) {
                    return (T)(object)int.Parse(value);
                } else if (typeof(T) == typeof(float)) {
                    return (T) (object) float.Parse(value);
                } else {
                    Debug.LogError("ParseUtils.Parse was parsed a T type that isn't setup to be parsed!");
                }

                return (T)(object)null;
            }
        }
    }
}

