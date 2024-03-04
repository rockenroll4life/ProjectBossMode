using UnityEngine;

public class ResourceCost {
    private readonly ResourceType costType;
    private readonly int cost;

    public ResourceType GetResourceType() => costType;

    public ResourceCost(ResourceType costType, int cost) {
        this.costType = costType;
        this.cost = cost;
    }

    public int GetCost(LivingEntity owner) {
        //  TODO: [Rock]: Incorporate mana cost reduction from attributes
        return cost;
    }
}

public abstract class AbilityBase {
    protected static readonly int LAYER_MASK_GROUND = LayerMask.GetMask("Ground");
    protected static readonly int LAYER_MASK_MOB = LayerMask.GetMask("Mob");
    protected static readonly int LAYER_MASK_PLAYER = LayerMask.GetMask("Player");
    protected static readonly ResourceCost FREE_RESOURCE_COST = new ResourceCost(ResourceType.Mana, 0);

    public enum TriggerType {
        Cast,
        Toggle,
        Channel,
    }
    
    protected Player owner;
    AbilityNum abilityID = AbilityNum.NONE;
    protected ResourceCost cost = FREE_RESOURCE_COST;

    public AbilityBase(Player owner, AbilityNum abilityNum) {
        this.owner = owner;
        SetAbilityID(abilityNum);
    }

    protected abstract string GetName();

    //  NOTE: [Rock]: Do we even need this at this point? Investigate...
    protected abstract TriggerType GetTriggerType();

    protected virtual bool InterruptsMovement() => false;
    public int GetAbilityID() => (int) abilityID;
    public AbilityNum GetAbilityNum() => abilityID;
    public ResourceCost GetResourceCost() => cost;
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
