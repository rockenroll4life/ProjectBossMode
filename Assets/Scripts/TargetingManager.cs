using UnityEngine;

public class TargetingManager : MonoBehaviour {
    static TargetingManager targetingManager;
    public static TargetingManager instance {
        get {
            if (!targetingManager) {
                targetingManager = FindObjectOfType(typeof(TargetingManager)) as TargetingManager;

                if (!targetingManager) {
                    Debug.LogError("Using this requires an EventManager on a GameObject within the scene");
                }
            }
            return targetingManager;
        }
    }

    static readonly int RAYCAST_DISTANCE = 100;
    
    Camera cam;
    RaycastHit hit;
    bool validRaycastHit;

    private void Start() {
        cam = Camera.main;
    }

    public static bool IsValidHit(out RaycastHit hit) {
        hit = instance.hit;
        return instance.validRaycastHit;
    }

    private void Update() {
        //  Once a frame we'll cast out a ray to see what we hit so that we can cache the value and don't have to cast more than once
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        validRaycastHit = Physics.Raycast(ray, out hit, RAYCAST_DISTANCE);
    }
}
