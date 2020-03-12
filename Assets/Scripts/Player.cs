using UnityEngine;

public class Player : Entity {
    UI ui;

    protected override void Start() {
        base.Start();
        
        locomotion = gameObject.AddComponent<PlayerLocomotion>();
        ui = FindObjectOfType<UI>();
    }

    protected override void RegisterEvents() {
        AddEvent((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
        AddEvent((int) GameEvents.Health_Changed, HealthChanged);   //  Do we need to move Health Changed somewhere else?
    }

    protected override void UnregisterEvents() {
        RemoveEvent((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
        RemoveEvent((int) GameEvents.Health_Changed, HealthChanged);
    }

    void MouseLeftClick() {
        if (TargetingManager.IsValidHit(out RaycastHit hit)) {
            locomotion.SetDestination(hit.point);
        }
    }

    void HealthChanged() {
        ui.UpdateBar(UI.ResourceType.Health, (int) stats.HEALTH.currentValue, stats.HEALTH.GetPercent());
    }
}
