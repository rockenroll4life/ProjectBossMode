using System;
using UnityEngine;

public class AbilityManager {
    Entity owner;

    //
    public AbilityBase[] abilities = new AbilityBase[(int) AbilityNum.TOTAL_];

    public void Setup(Entity owner) {
        this.owner = owner;
    }

    public void Breakdown() {
        foreach (AbilityBase ability in abilities) {
            ability.Breakdown();
        }
    }

    public void SetAbility(AbilityNum abilityNum, Type ability) {
        abilities[(int) abilityNum] = (AbilityBase) Activator.CreateInstance(ability);
        abilities[(int) abilityNum].Setup(owner, "Ability " + abilityNum, 10);
        abilities[(int) abilityNum].SetAbilityID((int) abilityNum);
    }

    public void Update() {
        foreach(AbilityBase ability in abilities) {
            ability.Update();
        }
    }
}
