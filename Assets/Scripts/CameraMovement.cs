using RockUtils.GameEvents;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private static CameraMovement instance;
    public static CameraMovement Instance {
        get {
            if (instance == null) {
                instance = FindFirstObjectByType<CameraMovement>();
            }
            return instance;
        }
    }

    public const float MIN_ZOOM_DISTANCE = 10f;
    public const float MAX_ZOOM_DISTANCE = 40f;
    public const float ZOOM_SPEED = 5f;

    public const float DEFAULT_ZOOM_DISTANCE = 20f;
    public const float CAMERA_HEIGHT = 20f;
    public const float CAMERA_TILT = 65f;

    private LivingEntity cameraTarget;

    private Locomotion.MovementType movementType;
    private Vector3 dragOrigin;
    private bool isDragging = false;

    private float currentDistance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of CameraMovement found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    void Start() {
        EventManager.StartListening(GameEvents.Mouse_Right_Press, MouseRightPress);
        EventManager.StartListening(GameEvents.Mouse_Right_Release, MouseRightRelease);
        EventManager.StartListening(GameEvents.Mouse_Middle_Press, ResetZoom);
        EventManager.StartListening(GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
        EventManager.StartListening(GameEvents.KeyboardButton_Held + (int) KeyCode.Space, FocusOnTarget);

        currentDistance = DEFAULT_ZOOM_DISTANCE;
    }

    void OnDisable() {
        EventManager.StopListening(GameEvents.Mouse_Right_Press, MouseRightPress);
        EventManager.StopListening(GameEvents.Mouse_Right_Release, MouseRightRelease);
        EventManager.StopListening(GameEvents.Mouse_Middle_Press, ResetZoom);
        EventManager.StopListening(GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
        EventManager.StopListening(GameEvents.KeyboardButton_Held + (int) KeyCode.Space, FocusOnTarget);
    }

    public static void SetCameraTarget(LivingEntity cameraTarget) {
        Instance.movementType = cameraTarget.GetLocomotion().GetMovementType();
        Instance.cameraTarget = cameraTarget;

        Instance.UpdateZoomOffset(cameraTarget.transform.position);
    }

    private void Update() {
        if (cameraTarget == null) {
            return;
        }

        if (movementType != Locomotion.MovementType.Mouse) {
            FocusOnTarget(0);
        } else {
            if (isDragging) {
                Vector3 currentWorldPos = GetWorldPositionAtMouse();
                Vector3 difference = dragOrigin - currentWorldPos;

                transform.position += difference;
            }
        }
    }

    private void MouseRightPress(int param) {
        dragOrigin = GetWorldPositionAtMouse();
        isDragging = true;
    }

    private void MouseRightRelease(int param) {
        isDragging = false;
    }

    private void ResetZoom(int param) {
        currentDistance = DEFAULT_ZOOM_DISTANCE;
        UpdateZoomOffset(GetGroundPositionFromCamera());
    }

    private void ZoomInOut(int param) {
        float scroll = (param / 1000f);

        currentDistance -= scroll * ZOOM_SPEED;
        currentDistance = Mathf.Clamp(currentDistance, MIN_ZOOM_DISTANCE, MAX_ZOOM_DISTANCE);

        UpdateZoomOffset(GetGroundPositionFromCamera());
    }

    private void FocusOnTarget(int param) {
        UpdateZoomOffset(cameraTarget.transform.position);
    }

    private void UpdateZoomOffset(Vector3 target) {
        Quaternion rotation = Quaternion.Euler(CAMERA_TILT, 0f, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -currentDistance);

        transform.position = target + offset;
        transform.LookAt(target);
    }

    private Vector3 GetWorldPositionAtMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter)) {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }

    private Vector3 GetGroundPositionFromCamera() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out RaycastHit hit, MAX_ZOOM_DISTANCE, layerMask)) {
            return hit.point;
        }

        return Vector3.zero;
    }
}
