using UnityEngine;

public class AbilityButtons : MonoBehaviour {
    LivingEntity owner;

    public AbilityButton[] abilities = new AbilityButton[(int) AbilityNum._COUNT];

    public void Setup(LivingEntity owner, KeyCode[] defaultKeybindings) {
        this.owner = owner;

        AbilityManager abilityManager = owner.GetAbilities();
        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            AbilityNum abilityNum = (AbilityNum) i;
            abilities[i].Setup(owner, abilityManager.GetAbility(abilityNum), abilityNum, defaultKeybindings[i]);
        }
    }

    public void Breakdown() {
        foreach (AbilityButton button in abilities) {
            button.Breakdown();
        }
    }

    public void SetAbilityIcon(AbilityNum buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityNum buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
    }

    public void ResourceValueChanged(ResourceType type, float currentValue) {
        foreach (AbilityButton button in abilities) {
            ResourceCost resourceCost = button.GetAbility().GetResourceCost();
            ResourceType resourceType = resourceCost.GetResourceType();

            if (type == resourceType) {
                button.notEnoughResource.gameObject.SetActive(currentValue < resourceCost.GetCost(owner));
            }
        }
    }
}
