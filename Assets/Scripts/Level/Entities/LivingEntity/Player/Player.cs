using UnityEngine;
using RockUtils.GameEvents;
using System;

public abstract class Player : LivingEntity {
    public static readonly RangedAttribute ABILITY1_COOLDOWN = new("generic.ability1", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY2_COOLDOWN = new("generic.ability2", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY3_COOLDOWN = new("generic.ability3", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY4_COOLDOWN = new("generic.ability4", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ULTIMATE_COOLDOWN = new("generic.ultimate", 5, 0, float.MaxValue);

    public static readonly RangedAttribute[] ABILITY_COOLDOWNS = { ABILITY1_COOLDOWN, ABILITY2_COOLDOWN, ABILITY3_COOLDOWN, ABILITY4_COOLDOWN, ULTIMATE_COOLDOWN };

    static readonly Color PLAYER_COLOR = new(1f, 0.8431f, 0f);

    public GameObject UIPrefab;

    GameplayUI ui;
    AbilityManager abilities;
    SpellIndicators spellIndicators;

    //  Character related Prefabs and Textures
    public SpellIndicatorPrefabs spellIndicatorPrefabs;
    public AbilityTextures abilityTextures;

    protected float mana;

    public override EntityType GetEntityType() => EntityType.Player;
    public override Type GetSystemType() => typeof(Player);

    public SpellIndicators GetSpellIndicators() => spellIndicators;
    protected AbilityManager GetAbilities() => abilities;
    public float GetMana() => mana;

    protected override Color? GetHighlightColor() => PLAYER_COLOR;
    protected override Color? GetHighlightOutlineColor() { return PLAYER_COLOR; }

    public void UseMana(float mana) {
        this.mana = Mathf.Max(this.mana - mana, 0);
        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Mana_Changed, (int) (mana * 1000));
    }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        abilities = new AbilityManager(this);
        RegisterAbilities();
        
        targeter = new PlayerTargeter(this);
        spellIndicators = new SpellIndicators(this);

        ui = Instantiate(UIPrefab).GetComponent<GameplayUI>();
        ui.Setup(this, abilities);

        animator = new LivingEntityAnimator(this);
    }

    protected abstract void RegisterAbilities();

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
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        abilities.Update();
        spellIndicators.Update();

        float oldMana = mana;
        mana += Time.deltaTime * GetAttribute(LivingEntitySharedAttributes.MANA_REGEN_RATE).GetValue();
        mana = Mathf.Clamp(mana, 0, GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue());
        if (mana != oldMana) {
            EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Mana_Changed);
        }
    }
}
