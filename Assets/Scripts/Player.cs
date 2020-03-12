using UnityEngine;

public class Player : Entity {
    protected override void Start() {
        base.Start();
        
        locomotion = gameObject.AddComponent<PlayerLocomotion>();
    }

    protected override void RegisterEvents() {
        AddEvent((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
    }

    protected override void UnregisterEvents() {
        RemoveEvent((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
    }

    void MouseLeftClick() {
        if (TargetingManager.IsValidHit(out RaycastHit hit)) {
            locomotion.SetDestination(hit.point);
        }
    }
}
