﻿using System;
using UnityEngine;

namespace RockUtils {
    public class StringUtils {
        public static string GenerateUUID() {
            return Guid.NewGuid().ToString();
        }

        public static string ConcatenateStrings(string seperator, string[] strings) {
            return string.Join(seperator, strings);
        }

        public static void EditorRender3DText(string text, Vector3 worldPos, float oX = 0, float oY = 0, Color? colour = null) {

#if UNITY_EDITOR
            UnityEditor.Handles.BeginGUI();

            var restoreColor = GUI.color;

            if (colour.HasValue)
                GUI.color = colour.Value;
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }

            UnityEditor.Handles.Label(TransformByPixel(worldPos, oX, oY), text);

            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
#endif
        }

        static Vector3 TransformByPixel(Vector3 position, float x, float y) {
            return TransformByPixel(position, new Vector3(x, y));
        }

        static Vector3 TransformByPixel(Vector3 position, Vector3 translateBy) {
            Camera cam = UnityEditor.SceneView.currentDrawingSceneView.camera;
            if (cam)
                return cam.ScreenToWorldPoint(cam.WorldToScreenPoint(position) + translateBy);
            else
                return position;
        }
    }
}
