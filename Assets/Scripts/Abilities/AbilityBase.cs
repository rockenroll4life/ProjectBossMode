using UnityEngine;
using RockUtils.GameEvents;

//  TODO: [Rock]: Abilities don't need to be MonoBehaviours, we should let the ability manager handle them
public abstract class AbilityBase {
    public enum TriggerType {
        Instant,
        Toggle,
        Channel,
    }

    //  NOTE: [Rock]: For some reason the UI needs access to this variables. We need to break that dependency
    public Stat cooldown { private set; get; }
    
    protected AbilityNum abilityID = AbilityNum.NONE;

    protected bool interruptsMovement = false;

    protected abstract string GetName();
    protected virtual float GetCooldownTime() { return 0; }

    public TriggerType triggerType { protected set; get; }

    bool _toggled;
    public bool toggled {
        protected set => _toggled = value;
        get => triggerType == TriggerType.Toggle && _toggled;
    }

    Entity owner;

    protected virtual void RegisterEvents() {
        cooldown.SetValueUpdatedEvent((int) GameEvents.Ability_Cooldown_Update + (int) abilityID);
    }
    protected virtual void UnregisterEvents() {
        cooldown.RemoveEvent();
    }

    public virtual void Setup(Entity owner, AbilityNum abilityNum) {
        this.owner = owner;
        cooldown = new Stat("", GetCooldownTime(), 0, float.MaxValue);
        //  We're using this a little differently than normal...
        cooldown.currentValue = 0;

        SetAbilityID(abilityNum);
    }

    public virtual void Breakdown() {
        if (abilityID != AbilityNum.NONE) {
            UnregisterEvents();
        }
    }

    public void SetAbilityID(AbilityNum abilityID) {
        RemoveAbility();

        this.abilityID = abilityID;
        RegisterEvents();
    }

    public void RemoveAbility() {
        Breakdown();
        abilityID = AbilityNum.NONE;
    }

    protected void AttemptUseAbility(int param) {
        if ((cooldown == 0 || canBypassCooldown()) && CanUseAbility()) {
            UseAbility();
        }
    }

    protected virtual bool CanUseAbility() {
        return true;
    }
    protected virtual void UseAbility() {
        if (triggerType != TriggerType.Toggle) {
            cooldown.ResetCurrent();
        } else {
            EventManager.TriggerEvent((int) GameEvents.Ability_Toggle + (int) abilityID);
            toggled = !toggled;
        }

        if (interruptsMovement) {
            owner.locomotion.StopMovement();
        }
    }

    protected virtual bool canBypassCooldown() {
        return false;
    }

    //  Effects should use this override instead of the normal Update
    public virtual void Update() {
        if (cooldown > 0) {
            cooldown.currentValue -= Time.deltaTime;
        }
    }
}
