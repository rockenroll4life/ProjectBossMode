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

    public void RegisterAbility(AbilityBase ability) {
        abilities.Add(ability.GetAbilityBinding(), ability);
    }

    public AbilityBase GetAbility(Ability.Binding abilityBinding) => abilities.GetValueOrDefault(abilityBinding);
}
