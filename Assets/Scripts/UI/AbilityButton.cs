using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

[System.Serializable]
public class AbilityButton {
    public enum ID {
        None = -1,
        Ability1 = 0,
        Ability2 = 1,
        Ability3 = 2,
        Ability4 = 3,
        Ability5 = 4,
        _Count = 5
    }

    public Image icon;
    public Image cooldown;
    public Text cooldownTimeText;
    public Text keybindText;
    public Image auraIcon;
    
    ID abilityID = ID.None;
    KeyCode keybind = KeyCode.None;
    float maxCooldown = float.MaxValue;

    public void Setup(ID abilityID, KeyCode keybind) {
        SetAbilityID(abilityID);
        SetAbilityKeybind(keybind);
    }

    ~AbilityButton() {
        RemoveAbilityID();
    }

    public void SetAbilityID(ID abilityID) {
        RemoveAbilityID();

        this.abilityID = abilityID;
        EventManager.StartListening((int) GameEvents.Ability_Cooldown_Update + (int) abilityID, UpdateCooldown);
        EventManager.StartListening((int) GameEvents.Ability_Toggle + (int) abilityID, AbilityToggled);
    }

    public void SetValues(float maxCooldown) {
        this.maxCooldown = maxCooldown;
    }

    public void SetAbilityKeybind(KeyCode keybind) {
        RemoveAbilityKeybind();

        this.keybind = keybind;
        InputManager.AddInputListener(keybind, AbilityUsed);
        //  TODO: [Rock]: When we add support for Release and Held for buttons, add support for ability to listen for that as well
    }

    public void RemoveAbilityID() {
        if (abilityID != ID.None) {
            EventManager.StopListening((int) GameEvents.Ability_Cooldown_Update + (int) abilityID, UpdateCooldown);
            EventManager.StopListening((int) GameEvents.Ability_Toggle + (int) abilityID, AbilityToggled);
            abilityID = ID.None;
        }
    }

    public void RemoveAbilityKeybind() {
        if (keybind != KeyCode.None) {
            InputManager.RemoveInputListener(keybind, AbilityUsed);
            keybind = KeyCode.None;
        }
    }

    void UpdateCooldown(int param) {
        //  Grab the cooldown percent from the param and convert it back
        float percent = (param / 10000f);
        float curTime = maxCooldown * percent;
        //  We clamp this to an int value since it looks nicer and ceiling it so when it shows it hits 0, is when it actually does
        int displayValue = (int) Mathf.Ceil(curTime);

        if (percent > 0) {
            cooldown.gameObject.SetActive(true);
            cooldownTimeText.gameObject.SetActive(true);

            cooldown.fillAmount = percent;
            cooldownTimeText.text = displayValue.ToString();
        } else {
            cooldown.gameObject.SetActive(false);
            cooldownTimeText.gameObject.SetActive(false);
        }
    }

    void AbilityUsed(int param) {
        EventManager.TriggerEvent((int) GameEvents.Ability_Press + (int) abilityID);
    }

    void AbilityToggled(int param) {
        auraIcon.gameObject.SetActive(!auraIcon.gameObject.activeSelf);
    }
}
