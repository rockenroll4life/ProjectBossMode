using System;
using System.Collections.Generic;

public class Abilities {
    readonly LivingEntity owner;
    readonly Dictionary<Ability.Binding, AbilityBase> abilities = new();

    public Abilities(LivingEntity owner) {
        this.owner = owner;
    }

    public void Breakdown() {
        foreach (AbilityBase ability in abilities.Values) {
            ability.Breakdown();
        }
    }

    public void RegisterAbility(Ability.ID abilityID, Ability.Binding abilityBinding) {
        AbilityBase newAbility = (AbilityBase) Activator.CreateInstance(Ability.Info.GetAbilityClass(abilityID), owner, abilityID, abilityBinding);
        abilities.Add(abilityBinding, newAbility);
    }

    public AbilityBase GetAbility(Ability.Binding abilityBinding) => abilities.GetValueOrDefault(abilityBinding);

    public void Update() {
        foreach (AbilityBase ability in abilities.Values) {
            ability.Update();
        }
    }
}
