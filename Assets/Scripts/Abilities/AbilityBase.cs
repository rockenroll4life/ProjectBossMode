public abstract class AbilityBase {
    public enum TriggerType {
        Cast,
        Toggle,
        Channel,
    }
    
    protected LivingEntity owner;
    protected AbilityNum abilityID = AbilityNum.NONE;

    protected bool interruptsMovement = false;

    public AbilityBase(Player owner, AbilityNum abilityNum) {
        this.owner = owner;
        SetAbilityID(abilityNum);
    }

    protected abstract string GetName();
    protected virtual TriggerType GetTriggerType() { return TriggerType.Cast; }

    protected virtual void RegisterEvents() {
        //cooldown.SetValueUpdatedEvent((int) GameEvents.Ability_Cooldown_Update + (int) abilityID);
    }
    protected virtual void UnregisterEvents() {
        //cooldown.RemoveEvent();
    }

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
}
