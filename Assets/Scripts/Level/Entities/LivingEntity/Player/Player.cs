﻿using UnityEngine;
using RockUtils.GameEvents;
using System;

public abstract class Player : LivingEntity {
    static readonly Color PLAYER_COLOR = new(1f, 0.8431f, 0f);

    public GameObject UIPrefab;

    //  Character related Prefabs and Textures
    public SpellIndicatorPrefabs spellIndicatorPrefabs;

    public override EntityType GetEntityType() => EntityType.Player;
    public override Type GetSystemType() => typeof(Player);

    protected override Color? GetHighlightColor() => PLAYER_COLOR;
    protected override Color? GetHighlightOutlineColor() { return PLAYER_COLOR; }

    public override void Setup(Level level) {
        base.Setup(level);

        CameraMovement.SetCameraTarget(this);
    }

    protected override void RegisterComponents() {
        base.RegisterComponents();

        //  TODO: [Rock]: We should have a general locamotion manger that we can use to get input and it changes
        //  based on the active input type.
        //locomotion = new MouseLocomotion(this);
        locomotion = new KeyboardLocomotion(this);
        //locomotion = new ControllerLocomotion(this);

        //  TODO: [Rock]: The same as above but for targetting
        targeter = new PlayerMouseTargeter(this);
        spellIndicators = new SpellIndicators(this);

        animator = new LivingEntityAnimator(this);

        InGameUI.EnablePlayerUI(UIPrefab, this);
        InGameUI.EnableBossHealthBar(this);
    }

    protected override void UnregisterComponents() {
        base.UnregisterComponents();

        abilities.Breakdown();
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        spellIndicators.Update();
    }
}
