using System.Collections.Generic;

public class Abilities {
    readonly LivingEntity owner;
    readonly Dictionary<AbilityNum, AbilityBase> abilities = new();

    public Abilities(LivingEntity owner) {
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
}
