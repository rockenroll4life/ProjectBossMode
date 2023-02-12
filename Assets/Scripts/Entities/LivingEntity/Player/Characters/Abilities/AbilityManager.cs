using System;
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

    //  TOOD: [Rock]: I think I want to change this to a dictionary...
    public AbilityBase[] abilities = new AbilityBase[(int) AbilityNum._COUNT];

    public AbilityManager(Player owner) {
        this.owner = owner;
    }

    public void Breakdown() {
        foreach (AbilityBase ability in abilities) {
            ability.Breakdown();
        }
    }

    public void SetAbility(AbilityNum abilityNum, Type ability) {
        abilities[(int) abilityNum] = (AbilityBase) Activator.CreateInstance(ability, owner, abilityNum);
    }

    public AbilityBase GetAbility(AbilityNum abilityNum) => abilities[(int) abilityNum];

    public void Update() {
        foreach(AbilityBase ability in abilities) {
            if (ability is CastAbilityBase castAbility) {
                castAbility.Update();
            }
        }
    }
}
