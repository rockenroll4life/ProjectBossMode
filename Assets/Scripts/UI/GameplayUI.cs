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

        bars[(int) ResourceType.Health].Setup(owner.GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue());
        bars[(int) ResourceType.Mana].Setup(owner.GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue());

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Health_Changed, HealthChanged);
        owner.GetAttributes().RegisterListener(LivingEntitySharedAttributes.HEALTH_MAX, MaxHealthChanged);

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Mana_Changed, ManaChanged);
        owner.GetAttributes().RegisterListener(LivingEntitySharedAttributes.MANA_MAX, MaxManaChanged);
    }

    public void Breakdown() {
        foreach (AbilityButton button in abilities) {
            button.Breakdown();
        }

        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Health_Changed, HealthChanged);
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Mana_Changed, ManaChanged);
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

    void HealthChanged(int param) {
        bars[(int) ResourceType.Health].UpdateCurrentValue(owner.GetHealth());
    }

    void MaxHealthChanged(int param) {
        float maxHealth = (int) (param / 1000f);
        bars[(int) ResourceType.Health].UpdateMaxValue(maxHealth);
    }

    void ManaChanged(int param) {
        float currentMana = owner.GetMana();
        bars[(int) ResourceType.Mana].UpdateCurrentValue(currentMana);

        foreach_AbilityButton(button => {
            //  TODO: At the moment Abilities only accept mana...but we should make them able to consume any Resource (Mana, Health, etc)
            button.notEnoughMana.gameObject.SetActive(currentMana < button.GetAbility().GetManaCost());
        });
    }

    void MaxManaChanged(int param) {
        float maxMana = (int) (param / 1000f);
        bars[(int) ResourceType.Mana].UpdateMaxValue(maxMana);
        float currentMana = bars[(int) ResourceType.Mana].Current();

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
