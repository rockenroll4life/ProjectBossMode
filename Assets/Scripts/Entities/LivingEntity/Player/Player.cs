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
}
