using System.Collections.Generic;

namespace RockUtils {
    namespace ContainerUtils {
        public class ContainerUtils {
            //  Fisher-Yates shuffle algorithm
            public static void ShuffleList<T>(List<T> list) {
                int n = list.Count;
                while (n > 1) {
                    n--;
                    int k = UnityEngine.Random.Range(0, n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }
    }
}