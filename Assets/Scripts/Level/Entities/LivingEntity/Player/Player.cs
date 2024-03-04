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

    public override EntityType GetEntityType() => EntityType.Player;
    public override Type GetSystemType() => typeof(Player);

    public SpellIndicators GetSpellIndicators() => spellIndicators;
    public AbilityManager GetAbilities() => abilities;

    protected override Color? GetHighlightColor() => PLAYER_COLOR;
    protected override Color? GetHighlightOutlineColor() { return PLAYER_COLOR; }

    public override void Setup(Level level) {
        base.Setup(level);

        CameraMovement.SetCameraTarget(this);
    }

    //  TODO: [Rock]: Remove this scaler and have the ResourceCost know it should scale it's value
    public void UseResource(ResourceCost cost, float scaler = 1f) {
        ResourceType resourceType = cost.GetResourceType();

        float value = Mathf.Max(GetResource(resourceType) - (cost.GetCost(this) * scaler), 0);
        SetResource(resourceType, value);

        if (resourceType == ResourceType.Mana) {
            EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Mana_Changed, (int) (value * 1000));
        } else if (resourceType == ResourceType.Health) {
            EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Health_Changed, (int) (value * 1000));
        }
    }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        abilities = new AbilityManager(this);
        RegisterAbilities();

        //locomotion = new MouseLocomotion(this);
        locomotion = new KeyboardLocomotion(this);

        targeter = new PlayerMouseTargeter(this);
        spellIndicators = new SpellIndicators(this);

        ui = Instantiate(UIPrefab).GetComponent<GameplayUI>();
        ui.Setup(this);

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
    }
}
