using UnityEngine;
using UnityEngine.UI;
using RockUtils.GameEvents;
using RockUtils.KeyCodeUtils;
using System;
using static RockUtils.GameEvents.InputManager;

[Serializable]
public struct ControllerIcons {
    public Sprite A;
    public Sprite B;
    public Sprite X;
    public Sprite Y;

    public Sprite Left;
    public Sprite Right;
    public Sprite Up;
    public Sprite Down;

    public Sprite Left_Trigger;
    public Sprite Right_Trigger;

    public Sprite Left_Bumper;
    public Sprite Right_Bumper;

    public Sprite Reset;
    public Sprite Select;

    public Sprite ControllerButtonToSprite(ControllerButtons button) {
        switch (button) {
            case ControllerButtons.A:               return A;
            case ControllerButtons.B:               return B;
            case ControllerButtons.X:               return X;
            case ControllerButtons.Y:               return Y;

            case ControllerButtons.Left:            return Left; 
            case ControllerButtons.Right:           return Right;
            case ControllerButtons.Up:              return Up;
            case ControllerButtons.Down:            return Down;

            case ControllerButtons.Left_Trigger:    return Left_Trigger;
            case ControllerButtons.Right_Trigger:   return Right_Trigger;

            case ControllerButtons.Left_Bumper:     return Left_Bumper;
            case ControllerButtons.Right_Bumper:    return Right_Bumper;

            case ControllerButtons.Reset:           return Reset;
            case ControllerButtons.Select:          return Select;

            default:                                return A;
        }
    }
}

public class AbilityButton : MonoBehaviour {
    public KeyBindingKeys bindingKey;
    public Image icon;
    public Image cooldown;
    public TMPro.TextMeshProUGUI cooldownTimeText;
    public TMPro.TextMeshProUGUI keybindText;
    public Image keybindControllerIcon;
    public Image auraIcon;
    public Image notEnoughResource;
    public ControllerIcons controllerIcons;

    LivingEntity owner;
    AbilityBase ability;
    Ability.Binding abilityBinding = Ability.Binding.NONE;
    KeyBinding keybind = null;
    float maxCooldown;

    bool channeling = false;

    bool destroying = false;

    public AbilityBase GetAbility() => ability;

    public void Setup(LivingEntity owner, AbilityBase ability, Ability.Binding abilityBinding) {
        EventManager.StartListening(GameEvents.Keybindings_Changed, KeyBindingsChanged);

        this.owner = owner;
        this.ability = ability;
        this.abilityBinding = abilityBinding;

        maxCooldown = owner.GetAttribute(AttributeTypes.Ability1Cooldown + (int) abilityBinding).GetValue();

        icon.sprite = AbilityManager.GetAbilityIcon(ability.GetID());

        if (owner.GetLocomotion().GetMovementType() != Locomotion.MovementType.Controller) {
            keybindText.gameObject.SetActive(true);
        } else {
            keybindControllerIcon.gameObject.SetActive(true);
        }

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
        if (keybind != null) {
            ButtonStopListening(keybind.keyboard);
            ControllerStopListening(keybind.controller);
            keybind = null;
            keybindText.text = "";
        }

        //  Then, if we have a new keybind, update our kind and then start listening for it
        keybind = Settings.GetKeyBinding(bindingKey);
        if (keybind != null && !destroying) {
            keybindText.text = KeyCodeUtils.ToCharacter(keybind.keyboard);
            keybindControllerIcon.sprite = controllerIcons.ControllerButtonToSprite(keybind.controller);
            ButtonStartListening(keybind.keyboard);
            ControllerStartListening(keybind.controller);
        }
    }

    void ButtonStopListening(KeyCode key) {
        EventManager.StopListening(GameEvents.KeyboardButton_Pressed + (int) key, AbilityPressed);
        EventManager.StopListening(GameEvents.KeyboardButton_Released + (int) key, AbilityReleased);
        EventManager.StopListening(GameEvents.KeyboardButton_Held + (int) key, AbilityHeld);
    }

    void ButtonStartListening(KeyCode key) {
        EventManager.StartListening(GameEvents.KeyboardButton_Pressed + (int) key, AbilityPressed);
        EventManager.StartListening(GameEvents.KeyboardButton_Released + (int) key, AbilityReleased);
        EventManager.StartListening(GameEvents.KeyboardButton_Held + (int) key, AbilityHeld);
    }

    void ControllerStopListening(ControllerButtons button) {
        EventManager.StopListening(GameEvents.Controller_Button_Press + (int) button, AbilityPressed);
        EventManager.StopListening(GameEvents.Controller_Button_Release + (int) button, AbilityReleased);
        EventManager.StopListening(GameEvents.Controller_Button_Held + (int) button, AbilityHeld);
    }

    void ControllerStartListening(ControllerButtons button) {
        EventManager.StartListening(GameEvents.Controller_Button_Press + (int) button, AbilityPressed);
        EventManager.StartListening(GameEvents.Controller_Button_Release + (int) button, AbilityReleased);
        EventManager.StartListening(GameEvents.Controller_Button_Held + (int) button, AbilityHeld);
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
