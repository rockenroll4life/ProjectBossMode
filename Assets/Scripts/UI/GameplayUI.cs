using UnityEngine;
using UnityEngine.UI;

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

    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._COUNT];
    public AbilityButton[] abilities = new AbilityButton[(int) AbilityNum._COUNT];

    public static Vector3 auraRotation;

    //  TODO: [Rock]: Support rebinding keys
    readonly KeyCode[] defaultKeybindings = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T };

    //  TODO: [Rock]: I don't think we should be passing the ability manager to the UI. Figure out a way to update the ability max cooldown without it needing to know about this
    public void Setup(AbilityManager abilityManager) {
        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            this.abilities[i].Setup((AbilityNum) i, defaultKeybindings[i]);
            this.abilities[i].SetDefaultMaxCooldown(abilityManager.abilities[i].cooldown.GetValue());
        }
    }

    public void Breakdown() {
        foreach (AbilityButton button in abilities) {
            button.Breakdown();
        }
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
}
