using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

public class GameplayUI : MonoBehaviour {
    const float AURA_ROTATION_SPEED = 90f;

    public enum ResourceType {
        Health = 0,
        Mana = 1,
        _COUNT = 2
    }
    
    [System.Serializable]
    public class ResourceBar {
        public Image barFill;
        public Text currentText;
        public Text maxText;
    }

    LivingEntity owner;
    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._COUNT];
    public AbilityButton[] abilities = new AbilityButton[(int) AbilityNum._COUNT];

    public static Vector3 auraRotation;

    //  TODO: [Rock]: Support rebinding keys
    readonly KeyCode[] defaultKeybindings = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T };

    //  TODO: [Rock]: I don't think we should be passing the ability manager to the UI. Figure out a way to update the ability max cooldown without it needing to know about this
    public void Setup(Player owner, AbilityManager abilityManager) {
        this.owner = owner;

        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            abilities[i].Setup(owner, (AbilityNum) i, defaultKeybindings[i]);
        }

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Health_Changed, HealthChanged);
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
    public void SetBarMax(ResourceType type, int amount) {
        bars[(int) type].maxText.text = amount.ToString();
    }

    public void UpdateBar(ResourceType type, int current, float percent) {
        bars[(int) type].currentText.text = current.ToString();
        bars[(int) type].barFill.fillAmount = percent;
    }

    //  Abilities
    public void SetAbilityIcon(AbilityNum buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityNum buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
    }

    void HealthChanged(int param) {
        //  TODO: [Rock]: Once we have Entity Data to store things such as health we'll pull from that data.
        float currentHealth = owner.GetAttribute(LivingEntitySharedAttributes.MAX_HEALTH).GetValue();
        float maxHealth = owner.GetAttribute(LivingEntitySharedAttributes.MAX_HEALTH).GetValue();
        float healthPercent = currentHealth / maxHealth;

        UpdateBar(GameplayUI.ResourceType.Health, (int) currentHealth, healthPercent);
    }

    void ManaChanged(int param) {
        float currentMana = owner.GetAttribute(Player.MAX_MANA).GetValue();
        float maxMana = owner.GetAttribute(Player.MAX_MANA).GetValue();
        float ManaPercent = currentMana / maxMana;

        UpdateBar(GameplayUI.ResourceType.Mana, (int) currentMana, ManaPercent);
    }
}
