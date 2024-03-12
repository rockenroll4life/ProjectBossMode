using UnityEngine;

public abstract class AbilityBase {
    protected static readonly int LAYER_MASK_GROUND = LayerMask.GetMask("Ground");
    protected static readonly int LAYER_MASK_MOB = LayerMask.GetMask("Mob");
    protected static readonly int LAYER_MASK_PLAYER = LayerMask.GetMask("Player");
    protected static readonly ResourceCost FREE_RESOURCE_COST = new ResourceCost(null, EntityDataType.Mana, 0);

    public enum TriggerType {
        Cast,
        Toggle,
        Channel,
    }
    
    protected LivingEntity owner;
    Ability.Binding abilityBinding = Ability.Binding.NONE;
    protected ResourceCost cost = FREE_RESOURCE_COST;

    public AbilityBase(LivingEntity owner, Ability.Binding abilityBinding) {
        this.owner = owner;
        SetAbilityID(abilityBinding);
    }

    protected abstract string GetName();

    //  NOTE: [Rock]: Do we even need this at this point? Investigate...
    protected abstract TriggerType GetTriggerType();

    public abstract Ability.ID GetID();

    protected virtual bool InterruptsMovement() => false;
    public int GetAbilityID() => (int) abilityBinding;
    public Ability.Binding GetAbilityBinding() => abilityBinding;
    public ResourceCost GetResourceCost() => cost;
    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected virtual void RegisterAttributes() { }

    public virtual void Breakdown() {
        if (abilityBinding != Ability.Binding.NONE) {
            UnregisterEvents();
        }
    }

    public void SetAbilityID(Ability.Binding abilityID) {
        RemoveAbility();

        this.abilityBinding = abilityID;
        RegisterEvents();
        RegisterAttributes();
    }

    public void RemoveAbility() {
        Breakdown();
        abilityBinding = Ability.Binding.NONE;
    }

    protected void AttemptUseAbility(int param) {
        if (param == (int) GetAbilityBinding()) {
            if (CanUseAbility()) {
                UseAbility();
            }
        }
    }

    protected virtual bool CanUseAbility() => false;

    protected virtual void UseAbility() {
        if (InterruptsMovement()) {
            owner.GetLocomotion().StopMovement();
        }
    }

    protected virtual bool canBypassCooldown() => false;

    protected void PutOnCooldown() {
        float baseCooldown = owner.GetAttribute(GetCooldownAttribute()).GetValue();
        float cooldownReduction = (owner.GetAttribute(AttributeTypes.CooldownReduction).GetValue() / 100f);

        float value = Mathf.Max(baseCooldown - (baseCooldown * cooldownReduction), 0);
        owner.SetEntityData(EntityDataType.Ability1_Cooldown + GetAbilityID(), value);
    }

    AttributeTypes GetCooldownAttribute() {
        return AttributeTypes.Ability1Cooldown + GetAbilityID();
    }
}
