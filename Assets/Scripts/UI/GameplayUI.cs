using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;
using System;

public class GameplayUI : MonoBehaviour {
    const float AURA_ROTATION_SPEED = 90f;

    public enum ResourceType {
        Health = 0,
        Mana = 1,
        _COUNT = 2
    }
    
    [Serializable]
    public class ResourceBar {
        public Image barFill;
        public Text currentText;
        public Text maxText;
    }

    Player owner;
    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._COUNT];

    //  TODO: [Rock]: Make the ability Buttons a prefab that we instantiate for each button
    public AbilityButton[] abilities = new AbilityButton[(int) AbilityNum._COUNT];

    public static Vector3 auraRotation;

    //  TODO: [Rock]: Support rebinding keys
    readonly KeyCode[] defaultKeybindings = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

    public void Setup(Player owner, AbilityManager abilityManager) {
        this.owner = owner;

        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            AbilityNum abilityNum = (AbilityNum) i;
            abilities[i].Setup(owner, abilityManager.GetAbility(abilityNum), abilityNum, defaultKeybindings[i]);
        }

        int healthMax = (int) owner.GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue();
        UpdateBar(ResourceType.Health, healthMax, healthMax);

        int manaMax = (int) owner.GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue();
        UpdateBar(ResourceType.Mana, manaMax, manaMax);

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Health_Changed, HealthChanged);
        owner.GetAttributes().RegisterListener(LivingEntitySharedAttributes.HEALTH_MAX, HealthChanged);

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Mana_Changed, ManaChanged);
        owner.GetAttributes().RegisterListener(LivingEntitySharedAttributes.MANA_MAX, ManaChanged);
    }

    public void Breakdown() {
        foreach (AbilityButton button in abilities) {
            button.Breakdown();
        }

        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Health_Changed, HealthChanged);
    }

    void Update() {
        auraRotation.z -= AURA_ROTATION_SPEED * Time.deltaTime;
    }

    //  Health / Resource Bars
    public void UpdateBar(ResourceType type, float current, float max) {
        bars[(int) type].currentText.text = ((int) current).ToString();
        bars[(int) type].maxText.text = ((int) max).ToString();
        bars[(int) type].barFill.fillAmount = current / max;
    }

    //  Abilities
    public void SetAbilityIcon(AbilityNum buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityNum buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
    }

    //  TODO: [Rock]: Update the maxHealth to just use the param value instead of fetching the value (param / 1000)
    void HealthChanged(int param) {
        //  TODO: [Rock]: Once we have Entity Data to store things such as health we'll pull from that data.
        float currentHealth = owner.GetHealth();
        float maxHealth = owner.GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue();

        UpdateBar(ResourceType.Health, currentHealth, maxHealth);
    }

    //  TODO: [Rock]: Update the maxMana to just use the param value instead of fetching the value (param / 1000)
    void ManaChanged(int param) {
        float currentMana = owner.GetMana();
        float maxMana = owner.GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue();

        UpdateBar(ResourceType.Mana, currentMana, maxMana);

        foreach_AbilityButton(button => {
            button.notEnoughMana.gameObject.SetActive(currentMana < button.GetAbility().GetManaCost());
        });
    }

    void foreach_AbilityButton(Action<AbilityButton> lambda) {
        foreach (AbilityButton ability in abilities) {
            lambda(ability);
        }
    }
}
