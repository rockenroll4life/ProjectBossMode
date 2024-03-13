using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockUtils {
    namespace StreamingAssetsUtils {
        public class StreamingAssetsUtils {
            public static Sprite LoadSprite(string path, int width, int height) {
                if (path == "") {
                    return null;
                }

                string filePath = $"{Application.streamingAssetsPath}/{path}";
                if (System.IO.File.Exists(filePath)) {
                    byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);

                    Texture2D tex = new Texture2D(width, height);
                    tex.LoadImage(pngBytes);

                    return Sprite.Create(tex, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f);
                }

                return null;
            }

            public static TextAsset LoadTextAsset(string path) {
                if (path == "") {
                    return null;
                }

                string filePath = $"{Application.streamingAssetsPath}/{path}";
                if (System.IO.File.Exists(filePath)) {
                    return new TextAsset(System.IO.File.ReadAllText(filePath));
                }

                return null;
            }
        }
    }
}
