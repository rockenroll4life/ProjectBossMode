using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RockUtils.GameEvents;

public class CameraMovement : MonoBehaviour {
    Vector3 defaultLocation;
    float cameraSpeed = 125f;

    void Start() {
        defaultLocation = transform.position;

        

        EventManager.StartListening((int) GameEvents.Mouse_Right_Move_X, MouseRightMoveX);
        EventManager.StartListening((int) GameEvents.Mouse_Right_Move_Y, MouseRightMoveY);
        InputManager.AddInputListener(KeyCode.Space, ResetPosition);
    }

    void OnDisable() {
        EventManager.StopListening((int) GameEvents.Mouse_Right_Move_X, MouseRightMoveX);
        EventManager.StopListening((int) GameEvents.Mouse_Right_Move_Y, MouseRightMoveY);
        InputManager.RemoveInputListener(KeyCode.Space, ResetPosition);
    }

    void MouseRightMoveX(int param) {
        float movement = -(param / 1000f) * cameraSpeed;
        transform.Translate(Vector3.right * movement * Time.deltaTime, Space.World);
    }

    void MouseRightMoveY(int param) {
        float movement = -(param / 1000f) * cameraSpeed;
        transform.Translate(Vector3.forward * movement * Time.deltaTime, Space.World);
    }

    void ResetPosition(int param) {
        transform.position = defaultLocation;
    }
}
