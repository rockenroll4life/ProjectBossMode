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
        AddEvent((int) GameEvents.Mana_Changed, ManaChanged);
    }

    protected override void UnregisterEvents() {
        RemoveEvent((int) GameEvents.Mouse_LeftClick, MouseLeftClick);
        RemoveEvent((int) GameEvents.Health_Changed, HealthChanged);
        RemoveEvent((int) GameEvents.Mana_Changed, ManaChanged);
    }

    void MouseLeftClick(int param) {
        if (TargetingManager.IsValidHit(out RaycastHit hit)) {
            locomotion.SetDestination(hit.point);
        }
    }

    void HealthChanged(int param) {
        // Update the players Health UI elements
        ui.UpdateBar(UI.ResourceType.Health, (int) stats.HEALTH.currentValue, stats.HEALTH.GetPercent());
    }

    void ManaChanged(int param) {
        ui.UpdateBar(UI.ResourceType.Mana, (int) stats.MANA.currentValue, stats.MANA.GetPercent());
    }
}
