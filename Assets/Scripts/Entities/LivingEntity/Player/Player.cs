using UnityEngine;
using RockUtils.GameEvents;

public class Player : LivingEntity {
    public static readonly RangedAttribute MAX_MANA = new("generic.mana", 100, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY1_COOLDOWN = new("generic.ability1", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY2_COOLDOWN = new("generic.ability2", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY3_COOLDOWN = new("generic.ability3", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY4_COOLDOWN = new("generic.ability4", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ULTIMATE_COOLDOWN = new("generic.ultimate", 5, 0, float.MaxValue);

    public static readonly RangedAttribute[] ABILITY_COOLDOWNS = { ABILITY1_COOLDOWN, ABILITY2_COOLDOWN, ABILITY3_COOLDOWN, ABILITY4_COOLDOWN, ULTIMATE_COOLDOWN };

    static readonly Color PLAYER_COLOR = new(1f, 0.8431f, 0f);

    GameplayUI ui;
    AbilityManager abilities;

    public override EntityType GetEntityType() { return EntityType.Player; }

    protected override Color? GetHighlightColor() { return PLAYER_COLOR; }
    protected override Color? GetHighlightOutlineColor() { return PLAYER_COLOR; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        AddOwnedEvent((int) GameEvents.Health_Changed, HealthChanged);   //  Do we need to move Health Changed somewhere else?
        AddOwnedEvent((int) GameEvents.Mana_Changed, ManaChanged);
        AddOwnedEvent((int) GameEvents.Targeted_Entity, TargetedEntity);
        AddOwnedEvent((int) GameEvents.Targeted_World, TargetedWorld);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        RemoveOwnedEvent((int) GameEvents.Health_Changed, HealthChanged);
        RemoveOwnedEvent((int) GameEvents.Mana_Changed, ManaChanged);
        RemoveOwnedEvent((int) GameEvents.Targeted_Entity, TargetedEntity);
        RemoveOwnedEvent((int) GameEvents.Targeted_World, TargetedWorld);
    }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        locomotion = new PlayerLocomotion(this);

        ui = FindObjectOfType<GameplayUI>();
        ui.Setup(this, abilities);

        abilities = new AbilityManager(this);
        targeter = new PlayerTargeter(this);

        abilities.SetAbility(AbilityNum.Ability1, typeof(TestNoStopAbility));
        abilities.SetAbility(AbilityNum.Ability2, typeof(TestAbility));
        abilities.SetAbility(AbilityNum.Ability3, typeof(TestAbility));
        abilities.SetAbility(AbilityNum.Ability4, typeof(TestAbility));
        abilities.SetAbility(AbilityNum.Ultimate, typeof(TestChannelAbility));

        animator = new PlayerAnimator(this);
    }

    protected override void UnregisterComponents() {
        base.UnregisterComponents();

        abilities.Breakdown();

        ui.Breakdown();
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        //  Register any unique attributes to this entity
        GetAttributes().RegisterAttribute(MAX_MANA);

        GetAttributes().RegisterAttribute(ABILITY1_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY2_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY3_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY4_COOLDOWN);
        GetAttributes().RegisterAttribute(ULTIMATE_COOLDOWN);

        //  Update any of the base attribute values for this entity
        GetAttribute(LivingEntitySharedAttributes.MAX_HEALTH).SetBaseValue(500);
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        abilities.Update();
    }

    void TargetedEntity(int param) {
        
    }

    void TargetedWorld(int param) {
        Vector3? location = targeter.GetTargetedLocation();
        if (location.HasValue) {
            locomotion.MoveToLocation(location.Value);
        }
    }

    void HealthChanged(int param) {
        //  TODO: [Rock]: Once we have Entity Data to store things such as health we'll pull from that data.
        float currentHealth = GetAttribute(LivingEntitySharedAttributes.MAX_HEALTH).GetValue();
        float maxHealth = GetAttribute(LivingEntitySharedAttributes.MAX_HEALTH).GetValue();
        float healthPercent = currentHealth / maxHealth;

        ui.UpdateBar(GameplayUI.ResourceType.Health, (int) currentHealth, healthPercent);
    }

    void ManaChanged(int param) {
        float currentMana = GetAttribute(MAX_MANA).GetValue();
        float maxMana = GetAttribute(MAX_MANA).GetValue();
        float ManaPercent = currentMana / maxMana;

        ui.UpdateBar(GameplayUI.ResourceType.Mana, (int) currentMana, ManaPercent);
    }
}
