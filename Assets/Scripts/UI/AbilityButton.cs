using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;
using RockUtils.KeyCodeUtils;

public class AbilityButton : MonoBehaviour {
    public KeyBindingKeys bindingKey;
    public Image icon;
    public Image cooldown;
    public Text cooldownTimeText;
    public Text keybindText;
    public Image auraIcon;
    public Image notEnoughResource;

    LivingEntity owner;
    AbilityBase ability;
    AbilityNum abilityID = AbilityNum.NONE;
    KeyCode keybind = KeyCode.None;
    float maxCooldown = 3;

    bool channeling = false;

    public void Setup(LivingEntity player, AbilityBase ability, AbilityNum abilityID) {
        EventManager.StartListening(GameEvents.Keybindings_Changed, KeyBindingsChanged);

        this.owner = player;
        this.ability = ability;
        this.abilityID = abilityID;
        RegisterEvents();
        UpdateAbilityKeybind();
    }

    public AbilityBase GetAbility() => ability;

    public void Breakdown() {
        UnregisterEvents();
        UpdateAbilityKeybind();
    }

    void RegisterEvents() {
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Cooldown_Update + (int) abilityID, UpdateCooldown);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Cooldown_Max_Update + (int) abilityID, UpdateMaxCooldown);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Toggle + (int) abilityID, AbilityToggled);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Channel_Start + (int) abilityID, AbilityChannelStart);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Channel_Stop + (int) abilityID, AbilityChannelStop);
    }

    void UnregisterEvents() {
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Cooldown_Update + (int) abilityID, UpdateCooldown);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Cooldown_Max_Update + (int) abilityID, UpdateMaxCooldown);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Toggle + (int) abilityID, AbilityToggled);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Channel_Start + (int) abilityID, AbilityChannelStart);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Channel_Stop + (int) abilityID, AbilityChannelStop);
    }

    void UpdateAbilityKeybind() {
        //  If we have a previous Keybind, stop listening for it
        if (keybind != KeyCode.None) {
            EventManager.StopListening(GameEvents.KeyboardButton_Pressed + (int) keybind, AbilityPressed);
            EventManager.StopListening(GameEvents.KeyboardButton_Released + (int) keybind, AbilityReleased);
            EventManager.StopListening(GameEvents.KeyboardButton_Held + (int) keybind, AbilityHeld);
            keybind = KeyCode.None;
            keybindText.text = "";
        }

        //  Then, if we have a new keybind, update our kind and then start listening for it
        keybind = Settings.GetKeyBinding(bindingKey);
        if (keybind != KeyCode.None) {
            keybindText.text = KeyCodeUtils.ToCharacter(keybind);
            EventManager.StartListening(GameEvents.KeyboardButton_Pressed + (int) keybind, AbilityPressed);
            EventManager.StartListening(GameEvents.KeyboardButton_Released + (int) keybind, AbilityReleased);
            EventManager.StartListening(GameEvents.KeyboardButton_Held + (int) keybind, AbilityHeld);
        }
    }

    void UpdateCooldown(int param) {
        if (!channeling) {
            //  Grab the cooldown percent from the param and convert it back
            float curTime = param / 1000f;
            float percent = (curTime / maxCooldown);

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
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Press + (int) abilityID);
    }

    void AbilityReleased(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Release + (int) abilityID);
    }

    void AbilityHeld(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Held + (int) abilityID);
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

    //  TODO: [Rock]: We now have access to the ability, just get the max Cooldown from it
    public void UpdateMaxCooldown(int param) {
        maxCooldown = param / 1000f;
    }

    void KeyBindingsChanged(int param) {
        UpdateAbilityKeybind();
    }
}
