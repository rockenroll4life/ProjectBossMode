using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockUtils {
    namespace ParseUtils {
        public class ParseUtils {
            public static T Parse<T>(string value) {
                if (typeof(T).IsEnum) {
                    return (T) System.Enum.Parse(typeof(T), value);
                } else if (typeof(T) == typeof(int)) {
                    return (T)(object)int.Parse(value);
                }

                return (T)(object)null;
            }
        }
    }
}

