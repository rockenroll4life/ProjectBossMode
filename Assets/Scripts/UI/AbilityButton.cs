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
    public Text resourceCost;
    public Image auraIcon;
    public Image notEnoughResource;

    LivingEntity owner;
    AbilityBase ability;
    Ability.Binding abilityBinding = Ability.Binding.NONE;
    KeyCode keybind = KeyCode.None;
    float maxCooldown;

    bool channeling = false;

    bool destroying = false;

    public AbilityBase GetAbility() => ability;

    public void Setup(LivingEntity owner, AbilityBase ability, Ability.Binding abilityBinding) {
        EventManager.StartListening(GameEvents.Keybindings_Changed, KeyBindingsChanged);

        this.owner = owner;
        this.ability = ability;
        this.abilityBinding = abilityBinding;

        int cost = ability.GetResourceCost().GetCost();
        resourceCost.text = cost > 0 ? cost.ToString() : "";

        maxCooldown = owner.GetAttribute(AttributeTypes.Ability1Cooldown + (int) abilityBinding).GetValue();

        icon.sprite = AbilityManager.GetAbilityIcon(ability.GetID());

        RegisterEvents();
        UpdateAbilityKeybind();
    }

    private void OnDestroy() {
        destroying = true;
        UnregisterEvents();
        UpdateAbilityKeybind();
    }

    void RegisterEvents() {
        owner.GetAttributes().RegisterListener(AttributeTypes.Ability1Cooldown + (int) abilityBinding, UpdateMaxCooldown);

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Entity_Data_Changed + (int) EntityDataType.Ability1_Cooldown + (int) abilityBinding, UpdateCooldown);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Toggle, AbilityToggled);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Channel_Start, AbilityChannelStart);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Channel_Stop, AbilityChannelStop);
    }

    void UnregisterEvents() {
        owner.GetAttributes().UnregisterListener(AttributeTypes.Ability1Cooldown + (int) abilityBinding, UpdateMaxCooldown);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Entity_Data_Changed + (int) EntityDataType.Ability1_Cooldown + (int) abilityBinding, UpdateCooldown);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Toggle, AbilityToggled);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Channel_Start, AbilityChannelStart);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Channel_Stop, AbilityChannelStop);
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
        if (keybind != KeyCode.None && !destroying) {
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
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Press, (int) abilityBinding);
    }

    void AbilityReleased(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Release, (int) abilityBinding);
    }

    void AbilityHeld(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Held, (int) abilityBinding);
    }

    void AbilityToggled(int param) {
        if (param == (int) abilityBinding) {
            auraIcon.gameObject.SetActive(!auraIcon.gameObject.activeSelf);
        }
    }

    void AbilityChannelStart(int param) {
        if (param == (int) abilityBinding) {
            channeling = true;
            cooldown.fillAmount = 100;
            cooldown.gameObject.SetActive(true);
        }
    }

    void AbilityChannelStop(int param) {
        if (param == (int) abilityBinding) {
            channeling = false;
            cooldown.gameObject.SetActive(false);
        }
    }

    //  TODO: [Rock]: We now have access to the ability, just get the max Cooldown from it
    public void UpdateMaxCooldown(int param) {
        maxCooldown = param / 1000f;
    }

    void KeyBindingsChanged(int param) {
        UpdateAbilityKeybind();
    }
}
