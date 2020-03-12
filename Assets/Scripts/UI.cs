using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public enum ResourceType {
        Health = 0,
        Resource = 1,
        _Count = 2
    }

    public enum AbilityButtonNum {
        Ability1 = 0,
        Ability2 = 1,
        Ability3 = 2,
        Ability4 = 3,
        _Count = 4
    }
    
    [System.Serializable]
    public class ResourceBar {
        public Image barFill;
        public Text currentText;
        public Text maxText;
    }

    [System.Serializable]
    public class AbilityButton {
        public Image icon;
        public Image cooldown;
        public Text cooldownTimeText;
        public Text keybindText;
    }

    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._Count];
    public AbilityButton[] abilities = new AbilityButton[(int) AbilityButtonNum._Count];

    //  Health / Resource Bars
    public void SetBarMax(ResourceType type, int amount) {
        bars[(int) type].maxText.text = amount.ToString();
    }

    public void UpdateBar(ResourceType type, int current, float percent) {
        bars[(int) type].currentText.text = current.ToString();
        bars[(int) type].barFill.fillAmount = percent;
    }

    //  Abilities
    public void SetAbilityIcon(AbilityButtonNum buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityButtonNum buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
    }

    public void UpdateCooldown(AbilityButtonNum buttonNum, int cooldown, float percent) {
        if (percent > 0) {
            abilities[(int) buttonNum].cooldown.gameObject.SetActive(true);
            abilities[(int) buttonNum].cooldownTimeText.gameObject.SetActive(true);

            abilities[(int) buttonNum].cooldown.fillAmount = percent;
            abilities[(int) buttonNum].cooldownTimeText.text = cooldown.ToString();
        } else {
            abilities[(int) buttonNum].cooldown.gameObject.SetActive(false);
            abilities[(int) buttonNum].cooldownTimeText.gameObject.SetActive(false);
        }
    }
}
