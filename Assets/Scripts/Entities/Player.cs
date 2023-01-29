using UnityEngine;
using RockUtils.GameEvents;

public class Player : Entity {
    GameplayUI ui;
    AbilityManager abilities;

    bool moving = false;

    protected override void Initialize() {
        base.Initialize();

        entityType = EntityType.Player;
        highlightColor = new Color(1f, 0.8431f, 0f);
    }

    public override TargetingManager.TargetType GetTargetType() { return TargetingManager.TargetType.Player; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        AddEvent((int) GameEvents.Mouse_Left_Release, MouseLeftReleased);
        AddEvent((int) GameEvents.Health_Changed, HealthChanged);   //  Do we need to move Health Changed somewhere else?
        AddEvent((int) GameEvents.Mana_Changed, ManaChanged);
        AddEvent((int) GameEvents.Targeted_Entity, TargetedEntity);
        AddEvent((int) GameEvents.Targeted_World, TargetedWorld);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        RemoveEvent((int) GameEvents.Mouse_Left_Release, MouseLeftReleased);
        RemoveEvent((int) GameEvents.Health_Changed, HealthChanged);
        RemoveEvent((int) GameEvents.Mana_Changed, ManaChanged);
        RemoveEvent((int) GameEvents.Targeted_Entity, TargetedEntity);
        RemoveEvent((int) GameEvents.Targeted_World, TargetedWorld);
    }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        locomotion = gameObject.AddComponent<PlayerLocomotion>();

        abilities = new AbilityManager();
        abilities.Setup(this);

        ui = FindObjectOfType<GameplayUI>();
        ui.Setup(abilities);

        animator = gameObject.AddComponent<PlayerAnimator>();
        animator.SetOwner(this);
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        abilities.Update();
    }

    void TargetedEntity(int param) {

    }

    void TargetedWorld(int param) {
        if (TargetingManager.IsValidHit(out RaycastHit hit)) {
            moving = true;
            locomotion.MoveToLocation(hit.point);
        }
    }

    void MouseLeftReleased(int param) {
        moving = false;
    }

    void HealthChanged(int param) {
        // Update the players Health UI elements
        ui.UpdateBar(GameplayUI.ResourceType.Health, (int) stats.HEALTH.currentValue, stats.HEALTH.GetPercent());
    }

    void ManaChanged(int param) {
        ui.UpdateBar(GameplayUI.ResourceType.Mana, (int) stats.MANA.currentValue, stats.MANA.GetPercent());
    }
}
