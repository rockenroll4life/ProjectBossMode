using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {
    public enum TriggerType {
        Instant,
        Toggle,
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

    public virtual void Setup(Entity owner, string name, float cooldownTime) {
        this.owner = owner;
        this.name = name;
        abilityID = -1;
        cooldown = new Stat("", cooldownTime, 0, float.MaxValue);
        //  We're using this a little differently than normal...
        cooldown.currentValue = 0;
        interruptsMovement = false;
    }

    //  TODO: [Rock]: We should probably have a way of cleaning up the ability event listeners
    
    public void SetAbilityID(int abilityID) {
        if (abilityID != -1) {
            EventManager.StopListening((int) GameEvents.Ability_Use + abilityID, AttemptUseAbility);
            cooldown.RemoveEvent();
        }

        this.abilityID = abilityID;
        EventManager.StartListening((int) GameEvents.Ability_Use + abilityID, AttemptUseAbility);
        cooldown.SetValueUpdatedEvent((int) GameEvents.Ability_Cooldown_Update + abilityID);
    }

    public void RemoveAbility() {
        if (abilityID != -1) {
            EventManager.StartListening((int) GameEvents.Ability_Use + abilityID, AttemptUseAbility);
            cooldown.RemoveEvent();
            abilityID = -1;
        }
    }

    void AttemptUseAbility(int param) {
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
        } else if (triggerType == TriggerType.Toggle) {
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
    protected virtual void OnUpdate() {
        if (cooldown > 0) {
            cooldown.currentValue -= Time.deltaTime;
        }
    }

    void Update() {
        OnUpdate();
    }
}
