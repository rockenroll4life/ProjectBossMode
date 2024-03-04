using UnityEngine;
using RockUtils.GameEvents;
using System;

public abstract class Player : LivingEntity {
    static readonly Color PLAYER_COLOR = new(1f, 0.8431f, 0f);

    public GameObject UIPrefab;

    GameplayUI ui;
    SpellIndicators spellIndicators;

    //  Character related Prefabs and Textures
    public SpellIndicatorPrefabs spellIndicatorPrefabs;
    public AbilityTextures abilityTextures;

    public override EntityType GetEntityType() => EntityType.Player;
    public override Type GetSystemType() => typeof(Player);

    public SpellIndicators GetSpellIndicators() => spellIndicators;

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

        //locomotion = new MouseLocomotion(this);
        locomotion = new KeyboardLocomotion(this);

        targeter = new PlayerMouseTargeter(this);
        spellIndicators = new SpellIndicators(this);

        ui = Instantiate(UIPrefab).GetComponent<GameplayUI>();
        ui.Setup(this);

        animator = new LivingEntityAnimator(this);
    }

    protected override void UnregisterComponents() {
        base.UnregisterComponents();

        abilities.Breakdown();

        ui.Breakdown();
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        spellIndicators.Update();
    }
}
