using System;
using UnityEngine;

public class AbilityManager {
    LivingEntity owner;

    //
    public AbilityBase[] abilities = new AbilityBase[(int) AbilityNum._COUNT];

    public void Setup(LivingEntity owner) {
        this.owner = owner;
    }

    public void Breakdown() {
        foreach (AbilityBase ability in abilities) {
            ability.Breakdown();
        }
    }

    public void SetAbility(AbilityNum abilityNum, Type ability) {
        abilities[(int) abilityNum] = (AbilityBase) Activator.CreateInstance(ability);
        abilities[(int) abilityNum].Setup(owner, abilityNum);
    }

    public void Update() {
        foreach(AbilityBase ability in abilities) {
            ability.Update();
        }
    }
}
