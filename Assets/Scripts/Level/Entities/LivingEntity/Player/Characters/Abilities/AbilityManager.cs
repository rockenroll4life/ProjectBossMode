﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AbilityTextures {
    public Texture2D ability1Texture;
    public Texture2D ability2Texture;
    public Texture2D ability3Texture;
    public Texture2D ability4Texture;
    public Texture2D ability5Texture;
}

public class AbilityManager {
    readonly Player owner;
    readonly Dictionary<AbilityNum, AbilityBase> abilities = new();

    public AbilityManager(Player owner) {
        this.owner = owner;
    }

    public void Breakdown() {
        foreach (AbilityBase ability in abilities.Values) {
            ability.Breakdown();
        }
    }

    public void RegisterAbility(AbilityBase ability) {
        abilities.Add(ability.GetAbilityNum(), ability);
    }

    public AbilityBase GetAbility(AbilityNum abilityNum) => abilities.GetValueOrDefault(abilityNum);

    public void Update() {
        foreach(AbilityBase ability in abilities.Values) {
            if (ability is CastAbilityBase castAbility) {
                castAbility.Update();
            }
        }
    }
}