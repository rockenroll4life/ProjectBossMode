using UnityEngine;
using RockUtils.GameEvents;

public class Player : LivingEntity {
    public static readonly RangedAttribute ABILITY1_COOLDOWN = new("generic.ability1", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY2_COOLDOWN = new("generic.ability2", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY3_COOLDOWN = new("generic.ability3", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY4_COOLDOWN = new("generic.ability4", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ULTIMATE_COOLDOWN = new("generic.ultimate", 5, 0, float.MaxValue);

    public static readonly RangedAttribute[] ABILITY_COOLDOWNS = { ABILITY1_COOLDOWN, ABILITY2_COOLDOWN, ABILITY3_COOLDOWN, ABILITY4_COOLDOWN, ULTIMATE_COOLDOWN };

    static readonly Color PLAYER_COLOR = new(1f, 0.8431f, 0f);

    GameplayUI ui;
    AbilityManager abilities;

    protected float mana;
    protected float manaRegenRate = 5;

    public override EntityType GetEntityType() { return EntityType.Player; }

    protected override Color? GetHighlightColor() { return PLAYER_COLOR; }
    protected override Color? GetHighlightOutlineColor() { return PLAYER_COLOR; }

    public float GetMana() { return mana; }

    public void UseMana(float mana) {
        this.mana = Mathf.Max(this.mana - mana, 0);
        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Mana_Changed, (int) (mana * 1000));
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

        animator = new LivingEntityAnimator(this);
    }

    protected override void UnregisterComponents() {
        base.UnregisterComponents();

        abilities.Breakdown();

        ui.Breakdown();
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        //  Register any unique attributes to this entity
        GetAttributes().RegisterAttribute(ABILITY1_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY2_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY3_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY4_COOLDOWN);
        GetAttributes().RegisterAttribute(ULTIMATE_COOLDOWN);

        //  Update any of the base attribute values for this entity
        GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).SetBaseValue(500);
        health = GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue();

        GetAttribute(LivingEntitySharedAttributes.MANA_MAX).SetBaseValue(100);
        mana = GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue();

        //  TODO: [Rock]: We need to add an attribute listener for the max health changing so we can update the UI
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        abilities.Update();

        float oldMana = mana;
        mana += Time.deltaTime * GetAttribute(LivingEntitySharedAttributes.MANA_REGEN_RATE).GetValue();
        mana = Mathf.Clamp(mana, 0, GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue());
        if (mana != oldMana) {
            EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Mana_Changed);
        }
    }
}
