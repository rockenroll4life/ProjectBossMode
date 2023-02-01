using UnityEngine;
using RockUtils.GameEvents;

public abstract class AbilityBase {
    public enum TriggerType {
        Cast,
        Toggle,
        Channel,
    }

    //  NOTE: [Rock]: For some reason the UI needs access to this variables. We need to break that dependency
    public Stat cooldown { private set; get; }
    
    protected AbilityNum abilityID = AbilityNum.NONE;

    protected bool interruptsMovement = false;

    protected abstract string GetName();
    protected virtual TriggerType GetTriggerType() { return TriggerType.Cast; }
    protected virtual float GetCooldownTime() { return 0; }

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
        if (CanUseAbility()) {
            UseAbility();
        }
    }

    protected virtual bool CanUseAbility() {
        return false;
    }

    protected virtual void UseAbility() {
        if (interruptsMovement) {
            owner.locomotion.StopMovement();
        }
    }

    protected virtual bool canBypassCooldown() {
        return false;
    }

    public virtual void Update() {
        //  TODO: [Rock]: We should not be updating the cooldown's value in the Ability. Investigate into allowing the cooldown stat to update itself
        if (cooldown > 0) {
            cooldown.currentValue -= Time.deltaTime;
        }
    }
}
