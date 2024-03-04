using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;

public class CameraMovement : MonoBehaviour {
    static CameraMovement instance;
    public static CameraMovement Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<CameraMovement>();
            }
            return instance;
        }
    }


    readonly Vector3 MIN_ZOOM = new Vector3(0f, 8.38f, -5f);
    readonly Vector3 MAX_ZOOM = new Vector3(0f, 17.4f, -11.3f);
    readonly float CAMERA_SPEED = 125f;
    readonly float MAX_CAMERA_SPEED = 500;
    readonly float ZOOM_SPEED = 50f;

    LivingEntity cameraTarget;
    float zoomAmount = 0f;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Debug.LogWarning("Duplicate instance of CameraMovement found. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    void Start() {
        EventManager.StartListening((int) GameEvents.Mouse_Right_Move_X, MouseRightMoveX);
        EventManager.StartListening((int) GameEvents.Mouse_Right_Move_Y, MouseRightMoveY);
        EventManager.StartListening((int) GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
        InputManager.AddInputListener(KeyCode.Space, FocusOnTarget);
    }

    void OnDisable() {
        EventManager.StopListening((int) GameEvents.Mouse_Right_Move_X, MouseRightMoveX);
        EventManager.StopListening((int) GameEvents.Mouse_Right_Move_Y, MouseRightMoveY);
        EventManager.StopListening((int) GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
        InputManager.RemoveInputListener(KeyCode.Space, FocusOnTarget);
    }

    public static void SetCameraTarget(LivingEntity cameraTarget) {
        Instance.cameraTarget = cameraTarget;
    }

    private void Update() {
        if (cameraTarget == null) {
            return;
        }

        if (cameraTarget.GetLocomotion().GetMovementType() != Locomotion.MovementType.Mouse) {
            FocusOnTarget(0);
        }

    }

    void MouseRightMoveX(int param) {
        if (cameraTarget.GetLocomotion().GetMovementType() == Locomotion.MovementType.Mouse) {
            float movement = -(param / 1000f) * Mathf.Lerp(CAMERA_SPEED, MAX_CAMERA_SPEED, zoomAmount);
            transform.Translate(Vector3.right * movement * Time.deltaTime, Space.World);
        }
    }

    void MouseRightMoveY(int param) {
        if (cameraTarget.GetLocomotion().GetMovementType() == Locomotion.MovementType.Mouse) {
            float movement = -(param / 1000f) * Mathf.Lerp(CAMERA_SPEED, MAX_CAMERA_SPEED, zoomAmount);
            transform.Translate(Vector3.forward * movement * Time.deltaTime, Space.World);
        }
    }

    void ZoomInOut(int param) {
        //  Find out the position we're looking at in the world
        Vector3 offsetPos = transform.position - Vector3.Lerp(MIN_ZOOM, MAX_ZOOM, zoomAmount);

        //  Calculate the new zoom value
        float value = -Mathf.Sign(param) * ZOOM_SPEED * Time.deltaTime;
        zoomAmount = Mathf.Clamp01(zoomAmount + value);

        //  Reposition the camera based off that position we're looking at in the world
        transform.position = offsetPos + Vector3.Lerp(MIN_ZOOM, MAX_ZOOM, zoomAmount);
    }

    void FocusOnTarget(int param) {
        transform.position = cameraTarget.transform.position + Vector3.Lerp(MIN_ZOOM, MAX_ZOOM, zoomAmount);
    }
}
