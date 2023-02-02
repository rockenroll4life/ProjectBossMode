using UnityEngine;
using RockUtils.GameEvents;

public class Player : LivingEntity {
    static readonly Color PLAYER_COLOR = new Color(1f, 0.8431f, 0f);

    GameplayUI ui;
    AbilityManager abilities;

    public override EntityType GetEntityType() { return EntityType.Player; }
    public override TargetingManager.TargetType GetTargetType() { return TargetingManager.TargetType.Player; }

    protected override Color? GetHighlightColor() { return PLAYER_COLOR; }
    protected override Color? GetHighlightOutlineColor() { return PLAYER_COLOR; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        AddEvent(entityID, (int) GameEvents.Health_Changed, HealthChanged);   //  Do we need to move Health Changed somewhere else?
        AddEvent(entityID, (int) GameEvents.Mana_Changed, ManaChanged);
        AddEvent(entityID, (int) GameEvents.Targeted_Entity, TargetedEntity);
        AddEvent(entityID, (int) GameEvents.Targeted_World, TargetedWorld);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        RemoveEvent(entityID, (int) GameEvents.Health_Changed, HealthChanged);
        RemoveEvent(entityID, (int) GameEvents.Mana_Changed, ManaChanged);
        RemoveEvent(entityID, (int) GameEvents.Targeted_Entity, TargetedEntity);
        RemoveEvent(entityID, (int) GameEvents.Targeted_World, TargetedWorld);
    }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        locomotion = gameObject.AddComponent<PlayerLocomotion>();

        abilities = new AbilityManager();
        abilities.Setup(this);

        abilities.SetAbility(AbilityNum.Ability1, typeof(TestNoStopAbility));
        abilities.SetAbility(AbilityNum.Ability2, typeof(TestAbility));
        abilities.SetAbility(AbilityNum.Ability3, typeof(TestAbility));
        abilities.SetAbility(AbilityNum.Ability4, typeof(TestAbility));
        abilities.SetAbility(AbilityNum.Ultimate, typeof(TestChannelAbility));

        ui = FindObjectOfType<GameplayUI>();
        ui.Setup(abilities);

        animator = gameObject.AddComponent<PlayerAnimator>();
        animator.SetOwner(this);
    }

    protected override void UnregisterComponents() {
        base.UnregisterComponents();

        abilities.Breakdown();

        ui.Breakdown();
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        abilities.Update();
    }

    void TargetedEntity(int param) {
        
    }

    void TargetedWorld(int param) {
        if (targetingManager.IsValidHit(out RaycastHit hit)) {
            locomotion.MoveToLocation(hit.point);
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
