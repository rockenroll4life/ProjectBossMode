using UnityEngine;

public class AbilityButtons : MonoBehaviour {
    LivingEntity owner;

    public AbilityButton[] abilities = new AbilityButton[(int) AbilityNum._COUNT];

    public void Setup(LivingEntity owner) {
        this.owner = owner;

        Abilities abilities = owner.GetAbilities();
        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            AbilityNum abilityNum = (AbilityNum) i;
            this.abilities[i].Setup(owner, abilities.GetAbility(abilityNum), abilityNum);
        }
    }

    public void SetAbilityIcon(AbilityNum buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityNum buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
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
