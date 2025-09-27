using UnityEngine;
using RockUtils.GameEvents;

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

    private readonly bool supportZoom = false;
    private readonly Vector3 DEFAULT_ZOOM_OFFSET = new Vector3(0f, 20f, -9f);
    private readonly Vector3 MIN_ZOOM_OFFSET = new Vector3(0f, 20f, -9f);
    private readonly Vector3 MAX_ZOOM_OFFSET = new Vector3(0f, 20f, -9f);

    private readonly float ZOOM_SPEED = 50f;

    private LivingEntity cameraTarget;
    private float zoomAmount = 0f;

    private Vector3 dragOrigin;
    private bool isDragging = false;

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

        if (supportZoom) {
            EventManager.StartListening(GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
            InputManager.AddInputListener(KeyCode.Space, FocusOnTarget);
        }
    }

    void OnDisable() {
        EventManager.StopListening(GameEvents.Mouse_Right_Press, MouseRightPress);
        EventManager.StopListening(GameEvents.Mouse_Right_Release, MouseRightRelease);

        if (supportZoom) {
            EventManager.StopListening(GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
            InputManager.RemoveInputListener(KeyCode.Space, FocusOnTarget);
        }
    }

    public static void SetCameraTarget(LivingEntity cameraTarget) {
        Instance.cameraTarget = cameraTarget;
        Instance.transform.position = cameraTarget.transform.position + Instance.GetZoomOffset();
    }

    private void Update() {
        if (cameraTarget == null) {
            return;
        }

        if (cameraTarget.GetLocomotion().GetMovementType() != Locomotion.MovementType.Mouse) {
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

    private void ZoomInOut(int param) {
        //  Find out the position we're looking at in the world
        Vector3 offsetPos = transform.position - GetZoomOffset();

        //  Calculate the new zoom value
        float value = -Mathf.Sign(param) * ZOOM_SPEED * Time.deltaTime;
        zoomAmount = Mathf.Clamp01(zoomAmount + value);

        //  Reposition the camera based off that position we're looking at in the world
        transform.position = offsetPos + GetZoomOffset();
    }

    private void FocusOnTarget(int param) {
        transform.position = cameraTarget.transform.position + GetZoomOffset();
    }

    private Vector3 GetZoomOffset() {
        if (supportZoom) {
            return Vector3.Lerp(MIN_ZOOM_OFFSET, MAX_ZOOM_OFFSET, zoomAmount);
        } else {
            return DEFAULT_ZOOM_OFFSET;
        }
    }

    private Vector3 GetWorldPositionAtMouse() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter)) {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }
}
