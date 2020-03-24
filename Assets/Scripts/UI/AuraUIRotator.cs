using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraUIRotator : MonoBehaviour {
    RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(GameplayUI.auraRotation);
    }

    void Update() {
        rectTransform.rotation = Quaternion.Euler(GameplayUI.auraRotation);
    }
}
