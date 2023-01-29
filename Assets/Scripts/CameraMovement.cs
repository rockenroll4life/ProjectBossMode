using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;

public class CameraMovement : MonoBehaviour {
    readonly Vector3 MIN_ZOOM = new Vector3(0f, 8.38f, -5f);
    readonly Vector3 MAX_ZOOM = new Vector3(0f, 17.4f, -11.3f);
    readonly float CAMERA_SPEED = 125f;
    readonly float MAX_CAMERA_SPEED = 500;
    readonly float ZOOM_SPEED = 50f;

    public Player player;
    float zoomAmount = 0f;

    void Start() {
        EventManager.StartListening((int) GameEvents.Mouse_Right_Move_X, MouseRightMoveX);
        EventManager.StartListening((int) GameEvents.Mouse_Right_Move_Y, MouseRightMoveY);
        EventManager.StartListening((int) GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
        InputManager.AddInputListener(KeyCode.Space, FocusOnPlayer);
    }

    void OnDisable() {
        EventManager.StopListening((int) GameEvents.Mouse_Right_Move_X, MouseRightMoveX);
        EventManager.StopListening((int) GameEvents.Mouse_Right_Move_Y, MouseRightMoveY);
        EventManager.StopListening((int) GameEvents.Mouse_Scroll_Wheel, ZoomInOut);
        InputManager.RemoveInputListener(KeyCode.Space, FocusOnPlayer);
    }

    void MouseRightMoveX(int param) {
        float movement = -(param / 1000f) * Mathf.Lerp(CAMERA_SPEED, MAX_CAMERA_SPEED, zoomAmount);
        transform.Translate(Vector3.right * movement * Time.deltaTime, Space.World);
    }

    void MouseRightMoveY(int param) {
        float movement = -(param / 1000f) * Mathf.Lerp(CAMERA_SPEED, MAX_CAMERA_SPEED, zoomAmount);
        transform.Translate(Vector3.forward * movement * Time.deltaTime, Space.World);
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

    void FocusOnPlayer(int param) {
        transform.position = player.transform.position + Vector3.Lerp(MIN_ZOOM, MAX_ZOOM, zoomAmount);
    }
}
