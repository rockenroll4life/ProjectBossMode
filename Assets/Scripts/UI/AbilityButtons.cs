using UnityEngine;

public class AbilityButtons : MonoBehaviour {
    LivingEntity owner;

    public AbilityButton[] abilities = new AbilityButton[Ability.Info.NUM_ABILITIES];

    public void Setup(LivingEntity owner) {
        this.owner = owner;

        Abilities abilities = owner.GetAbilities();
        for (int i = 0; i < Ability.Info.NUM_ABILITIES; i++) {
            Ability.Binding abilityBinding = (Ability.Binding) i;
            this.abilities[i].Setup(owner, abilities.GetAbility(abilityBinding), abilityBinding);
        }
    }

    public void ResourceValueChanged(EntityDataType type, float currentValue) {
        foreach (AbilityButton button in abilities) {
            ResourceCost resourceCost = button.GetAbility().GetResourceCost();
            EntityDataType resourceType = resourceCost.GetResourceType();

            if (type == resourceType) {
                button.notEnoughResource.gameObject.SetActive(currentValue < resourceCost.GetCost());
            }
        }
    }
}
