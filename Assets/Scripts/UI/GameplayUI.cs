using UnityEngine;

using RockUtils.GameEvents;
using System;

public class GameplayUI : MonoBehaviour {
    const float AURA_ROTATION_SPEED = 90f;

    Player owner;
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

        bars[(int) ResourceType.Health].Setup(owner, LivingEntitySharedAttributes.HEALTH_MAX, GameEvents.Health_Changed, null);
        bars[(int) ResourceType.Mana].Setup(owner, LivingEntitySharedAttributes.MANA_MAX, GameEvents.Mana_Changed, ManaValueChanged);
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

    void ManaValueChanged(float currentMana) {
        foreach_AbilityButton(button => {
            //  TODO: At the moment Abilities only accept mana...but we should make them able to consume any Resource (Mana, Health, etc)
            button.notEnoughMana.gameObject.SetActive(currentMana < button.GetAbility().GetManaCost());
        });
    }

    void foreach_AbilityButton(Action<AbilityButton> lambda) {
        foreach (AbilityButton ability in abilities) {
            lambda(ability);
        }
    }
}
