﻿using UnityEngine;
using RockUtils.GameEvents;

public class Player : LivingEntity {
    public static readonly RangedAttribute MAX_MANA = new RangedAttribute("generic.mana", 100, 0, float.MaxValue);

    static readonly Color PLAYER_COLOR = new Color(1f, 0.8431f, 0f);

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

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        //  Register any unique attributes to this entity
        GetAttributes().RegisterAttribute(MAX_MANA);

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
        if (targetingManager.IsValidHit(out RaycastHit hit)) {
            locomotion.MoveToLocation(hit.point);
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