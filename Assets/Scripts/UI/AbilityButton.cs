using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;

[System.Serializable]
public class AbilityButton {
    public Image icon;
    public Image cooldown;
    public Text cooldownTimeText;
    public Text keybindText;
    public Image auraIcon;

    AbilityNum abilityID = AbilityNum.NONE;
    KeyCode keybind = KeyCode.None;
    float maxCooldown = float.MaxValue;

    bool channeling = false;

    public void SetDefaultMaxCooldown(float maxCooldown) {
        this.maxCooldown = maxCooldown;
    }

    public void Setup(AbilityNum abilityID, KeyCode keybind) {
        this.abilityID = abilityID;
        RegisterEvents();
        UpdateAbilityKeybind(keybind);
    }

    public void Breakdown() {
        UnregisterEvents();
        UpdateAbilityKeybind(KeyCode.None);
    }

    void RegisterEvents() {
        EventManager.StartListening((int) GameEvents.Ability_Cooldown_Update + (int) abilityID, UpdateCooldown);
        EventManager.StartListening((int) GameEvents.Ability_Toggle + (int) abilityID, AbilityToggled);
        EventManager.StartListening((int) GameEvents.Ability_Channel_Start + (int) abilityID, AbilityChannelStart);
        EventManager.StartListening((int) GameEvents.Ability_Channel_Stop + (int) abilityID, AbilityChannelStop);
    }

    void UnregisterEvents() {
        EventManager.StopListening((int) GameEvents.Ability_Cooldown_Update + (int) abilityID, UpdateCooldown);
        EventManager.StopListening((int) GameEvents.Ability_Toggle + (int) abilityID, AbilityToggled);
        EventManager.StopListening((int) GameEvents.Ability_Channel_Start + (int) abilityID, AbilityChannelStart);
        EventManager.StopListening((int) GameEvents.Ability_Channel_Stop + (int) abilityID, AbilityChannelStop);
    }

    void UpdateAbilityKeybind(KeyCode keybind) {
        //  TODO: [Rock]: When we add support for Release and Held for buttons, add support for ability to listen for that as well

        //  If we have a previous Keybind, stop listening for it
        if (this.keybind != KeyCode.None) {
            EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) this.keybind, AbilityPressed);
            EventManager.StopListening((int) GameEvents.KeyboardButton_Released + (int) this.keybind, AbilityReleased);
            EventManager.StopListening((int) GameEvents.KeyboardButton_Held + (int) this.keybind, AbilityHeld);
            this.keybind = KeyCode.None;
        }

        //  Then, if we have a new keybind, update our kind and then start listening for it
        if (keybind != KeyCode.None) {
            this.keybind = keybind;
            EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) this.keybind, AbilityPressed);
            EventManager.StartListening((int) GameEvents.KeyboardButton_Released + (int) this.keybind, AbilityReleased);
            EventManager.StartListening((int) GameEvents.KeyboardButton_Held + (int) this.keybind, AbilityHeld);
        }
    }

    void UpdateCooldown(int param) {
        if (!channeling) {
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
    }

    void AbilityPressed(int param) {
        EventManager.TriggerEvent((int) GameEvents.Ability_Press + (int) abilityID);
    }

    void AbilityReleased(int param) {
        EventManager.TriggerEvent((int) GameEvents.Ability_Release + (int) abilityID);
    }

    void AbilityHeld(int param) {
        EventManager.TriggerEvent((int) GameEvents.Ability_Held + (int) abilityID);
    }

    void AbilityToggled(int param) {
        auraIcon.gameObject.SetActive(!auraIcon.gameObject.activeSelf);
    }

    void AbilityChannelStart(int param) {
        channeling = true;
        cooldown.fillAmount = 100;
        cooldown.gameObject.SetActive(true);
    }

    void AbilityChannelStop(int param) {
        channeling = false;
        cooldown.gameObject.SetActive(false);
    }
}
