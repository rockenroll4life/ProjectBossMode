using RockUtils.GameEvents;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastAbilityBase : AbilityBase {
    bool isCasting = false;

    protected virtual bool BypassSpellIndicators() => false;

    public CastAbilityBase(LivingEntity owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base(owner, abilityID, abilityBinding) {
    }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Targeted_World, MoveCancelAbility);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Targeted_Entity, CancelAbility);
        EventManager.StartListening(GameEvents.KeyboardButton_Pressed + (int) KeyCode.Escape, CancelAbility);

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Press, AbilityPressed);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), GameEvents.Targeted_World, MoveCancelAbility);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Targeted_Entity, CancelAbility);
        EventManager.StopListening(GameEvents.KeyboardButton_Pressed + (int) KeyCode.Escape, CancelAbility);

        EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Press, AbilityPressed);
    }

    protected override bool CanUseAbility() {
        if (canBypassCooldown())
            return true;

        if (owner.GetEntityData(EntityDataType.Ability1_Cooldown + GetAbilityID()) <= 0) {
            ResourceCost resourceCost = GetResourceCost();
            EntityDataType resourceType = resourceCost.GetResourceType();

            return owner.GetEntityData(resourceType) >= resourceCost.GetCost();
        }

        return false;
    }

    protected override void UseAbility() {
        base.UseAbility();

        if (!isCasting && !BypassSpellIndicators()) {
            isCasting = true;
            ShowSpellIndicator();
        } else {
            isCasting = false;
            owner.GetSpellIndicators().ResetIndicators();

            CastAbility();
        }
    }

    protected virtual void CastAbility() {
        PutOnCooldown();
        owner.UseResource(GetResourceCost());
    }

    protected abstract void ShowSpellIndicator();

    void MoveCancelAbility(int param) {
        if (InterruptsMovement()) {
            CancelAbility(param);
        }
    }

    void CancelAbility(int param) {
        if (isCasting) {
            isCasting = false;
            owner.GetSpellIndicators().ResetIndicators();
        }
    }

    void AbilityPressed(int param) {
        if (param == (int) GetAbilityBinding()) {
            AttemptUseAbility(param);
        } else {
            OtherAbilityPressed(param);
        }
    }

    void OtherAbilityPressed(int param) {
        isCasting = false;
    }

    protected abstract List<Entity> GetEntitiesHitByAbility(int layerMask);
}
