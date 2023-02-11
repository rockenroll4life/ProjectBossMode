public abstract class AbilityBase {
    public enum TriggerType {
        Cast,
        Toggle,
        Channel,
    }
    
    protected Player owner;
    protected AbilityNum abilityID = AbilityNum.NONE;

    public AbilityBase(Player owner, AbilityNum abilityNum) {
        this.owner = owner;
        SetAbilityID(abilityNum);
    }

    protected abstract string GetName();

    //  NOTE: [Rock]: Do we even need this at this point? Investigate...
    protected abstract TriggerType GetTriggerType();

    protected virtual bool InterruptsMovement() => false;

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected virtual void RegisterAttributes() { }

    public virtual void Breakdown() {
        if (abilityID != AbilityNum.NONE) {
            UnregisterEvents();
        }
    }

    public void SetAbilityID(AbilityNum abilityID) {
        RemoveAbility();

        this.abilityID = abilityID;
        RegisterEvents();
        RegisterAttributes();
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

    protected virtual bool CanUseAbility() => false;

    protected virtual void UseAbility() {
        if (InterruptsMovement()) {
            owner.GetLocomotion().StopMovement();
        }
    }

    protected virtual bool canBypassCooldown() => false;
}
