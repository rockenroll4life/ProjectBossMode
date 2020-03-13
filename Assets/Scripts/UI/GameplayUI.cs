using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour {
    public enum ResourceType {
        Health = 0,
        Mana = 1,
        _Count = 2
    }
    
    [System.Serializable]
    public class ResourceBar {
        public Image barFill;
        public Text currentText;
        public Text maxText;
    }

    public ResourceBar[] bars = new ResourceBar[(int) ResourceType._Count];
    public AbilityButton[] abilities = new AbilityButton[(int) AbilityButton.ID._Count];

    //  TODO: [Rock]: Support rebinding keys
    readonly KeyCode[] defaultKeybindings = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T };

    //  TODO: [Rock]: I don't think we should be passing the ability manager to the UI. Figure out a way to update the ability max cooldown without it needing to know about this
    public void Setup(AbilityManager abilities) {
        for (int i = 0; i < (int) AbilityButton.ID._Count; i++) {
            this.abilities[i].Setup((AbilityButton.ID) i, defaultKeybindings[i]);
            this.abilities[i].SetValues(abilities.abilities[i].cooldown.GetValue());
        }
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
    public void SetAbilityIcon(AbilityButton.ID buttonNum, Sprite sprite) {
        abilities[(int) buttonNum].icon.sprite = sprite;
    }

    public void SetAbilityKeybind(AbilityButton.ID buttonNum, string keybind) {
        abilities[(int) buttonNum].keybindText.text = keybind;
    }
}
