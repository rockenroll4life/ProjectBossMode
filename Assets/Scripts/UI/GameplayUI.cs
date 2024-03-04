using UnityEngine;
using System;
using RockUtils.GameEvents;

public class GameplayUI : MonoBehaviour {
    const float AURA_ROTATION_SPEED = 90f;

    private Player owner;

    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._COUNT];

    //  TODO: [Rock]: Make the ability Buttons a prefab that we instantiate for each button
    public AbilityButton[] abilities = new AbilityButton[(int) AbilityNum._COUNT];

    public static Vector3 auraRotation;

    //  TODO: [Rock]: Support rebinding keys
    readonly KeyCode[] defaultKeybindings = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    public void Setup(Player owner) {
        this.owner = owner;
        AbilityManager abilityManager = owner.GetAbilities();
        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            AbilityNum abilityNum = (AbilityNum) i;
            abilities[i].Setup(owner, abilityManager.GetAbility(abilityNum), abilityNum, defaultKeybindings[i]);
        }

        bars[(int) ResourceType.Health].Setup(owner, ResourceType.Health, LivingEntitySharedAttributes.HEALTH_MAX, GameEvents.Health_Changed, ResourceValueChanged);
        bars[(int) ResourceType.Mana].Setup(owner, ResourceType.Mana, LivingEntitySharedAttributes.MANA_MAX, GameEvents.Mana_Changed, ResourceValueChanged);
    }

    public void Breakdown() {
        foreach (AbilityButton button in abilities) {
            button.Breakdown();
        }

       foreach(ResourceBar bar in bars) {
            bar.Breakdown();
        }
    }

    void Update() {
        auraRotation.z -= AURA_ROTATION_SPEED * Time.deltaTime;
    }

    //  Abilities
    public void SetAbilityIcon(AbilityNum buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityNum buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
    }

    void ResourceValueChanged(ResourceType type, float currentValue) {
        foreach (AbilityButton button in abilities) {
            ResourceCost resourceCost = button.GetAbility().GetResourceCost();
            ResourceType resourceType = resourceCost.GetResourceType();

            if (type == resourceType) {
                button.notEnoughResource.gameObject.SetActive(currentValue < resourceCost.GetCost(owner));
            }
        }
    }
}
