using System;

public class AbilityManager {
    readonly Player owner;

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

    public void Update() {
        foreach(AbilityBase ability in abilities) {
            if (ability is CastAbilityBase castAbility) {
                castAbility.Update();
            }
        }
    }
}
