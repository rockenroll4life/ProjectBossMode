using UnityEngine;

public class Player : Entity {
    GameplayUI ui;
    AbilityManager abilities;

    bool moving = false;

    protected override void Start() {
        base.Start();
        
        locomotion = gameObject.AddComponent<PlayerLocomotion>();

        abilities = gameObject.AddComponent<AbilityManager>();
        abilities.Setup();

        ui = FindObjectOfType<GameplayUI>();
        ui.Setup(abilities);
    }

    protected override void RegisterEvents() {
        AddEvent((int) GameEvents.Mouse_Left_Press, MouseLeftPressed);
        AddEvent((int) GameEvents.Mouse_Left_Release, MouseLeftReleased);
        AddEvent((int) GameEvents.Mouse_Left_Held, MouseLeftHeld);
        AddEvent((int) GameEvents.Health_Changed, HealthChanged);   //  Do we need to move Health Changed somewhere else?
        AddEvent((int) GameEvents.Mana_Changed, ManaChanged);
    }

    protected override void UnregisterEvents() {
        RemoveEvent((int) GameEvents.Mouse_Left_Press, MouseLeftPressed);
        RemoveEvent((int) GameEvents.Mouse_Left_Release, MouseLeftReleased);
        RemoveEvent((int) GameEvents.Mouse_Left_Held, MouseLeftHeld);
        RemoveEvent((int) GameEvents.Health_Changed, HealthChanged);
        RemoveEvent((int) GameEvents.Mana_Changed, ManaChanged);
    }

    void MouseLeftPressed(int param) {
        if (TargetingManager.IsValidHit(out RaycastHit hit)) {
            if (TargetingManager.GetHitType() == TargetingManager.TargetType.World) {
                moving = true;
                locomotion.MoveToLocation(hit.point);
            }

        }
    }

    void MouseLeftReleased(int param) {
        moving = false;
    }

    void MouseLeftHeld(int param) {
        if (moving) {
            locomotion.MoveToLocation(TargetingManager.GetHitLocation());
        }
    }

    void HealthChanged(int param) {
        // Update the players Health UI elements
        ui.UpdateBar(GameplayUI.ResourceType.Health, (int) stats.HEALTH.currentValue, stats.HEALTH.GetPercent());
    }

    void ManaChanged(int param) {
        ui.UpdateBar(GameplayUI.ResourceType.Mana, (int) stats.MANA.currentValue, stats.MANA.GetPercent());
    }
}
