using UnityEngine;

public class AuraUIRotator : MonoBehaviour {
    static readonly float AURA_ROTATION_SPEED = -90f;

    RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, AURA_ROTATION_SPEED * Time.realtimeSinceStartup));
    }
}
