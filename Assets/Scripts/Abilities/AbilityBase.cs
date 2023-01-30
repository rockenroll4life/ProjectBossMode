using UnityEngine;
using RockUtils.GameEvents;

//  TODO: [Rock]: Abilities don't need to be MonoBehaviours, we should let the ability manager handle them
public abstract class AbilityBase {
    public enum TriggerType {
        Instant,
        Toggle,
        Channel,
    }

    public string name { private set; get; }
    public Stat cooldown { private set; get; }
    //  This is used as an 'id' for this ability for a 1-10 value (What key it's bound to
    public int abilityID { private set; get; }

    public bool interruptsMovement { protected set; get; }

    public TriggerType triggerType { protected set; get; }

    bool _toggled;
    public bool toggled {
        protected set {
            _toggled = value;
        }
        get {
            return triggerType == TriggerType.Toggle && _toggled;
        }
    }

    Entity owner;

    protected virtual void RegisterEvents() {
        cooldown.SetValueUpdatedEvent((int) GameEvents.Ability_Cooldown_Update + abilityID);
    }
    protected virtual void UnregisterEvents() {
        cooldown.RemoveEvent();
    }

    public virtual void Setup(Entity owner, string name, float cooldownTime) {
        this.owner = owner;
        this.name = name;
        abilityID = -1;
        cooldown = new Stat("", cooldownTime, 0, float.MaxValue);
        //  We're using this a little differently than normal...
        cooldown.currentValue = 0;
        interruptsMovement = false;
    }

    public virtual void Breakdown() {
        if (abilityID != -1) {
            UnregisterEvents();
        }
    }

    public void SetAbilityID(int abilityID) {
        RemoveAbility();

        this.abilityID = abilityID;
        RegisterEvents();
    }

    public void RemoveAbility() {
        Breakdown();
        abilityID = -1;
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
            EventManager.TriggerEvent((int) GameEvents.Ability_Toggle + abilityID);
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
