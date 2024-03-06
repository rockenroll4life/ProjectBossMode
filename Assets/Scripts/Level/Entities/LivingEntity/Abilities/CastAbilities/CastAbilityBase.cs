using RockUtils.GameEvents;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastAbilityBase : AbilityBase {
    bool isCasting = false;

    public CastAbilityBase(LivingEntity owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override TriggerType GetTriggerType() => TriggerType.Cast;

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), GameEvents.Targeted_World, MoveCancelAbility);
        EventManager.StartListening(owner.GetEntityID(), GameEvents.Targeted_Entity, CancelAbility);
        EventManager.StartListening(GameEvents.KeyboardButton_Pressed + (int) KeyCode.Escape, CancelAbility);

        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            if (i == GetAbilityID()) {
                EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Press + i, AttemptUseAbility);
            } else {
                EventManager.StartListening(owner.GetEntityID(), GameEvents.Ability_Press + i, OtherAbilityPressed);
            }
        }
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), GameEvents.Targeted_World, MoveCancelAbility);
        EventManager.StopListening(owner.GetEntityID(), GameEvents.Targeted_Entity, CancelAbility);
        EventManager.StopListening(GameEvents.KeyboardButton_Pressed + (int) KeyCode.Escape, CancelAbility);

        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            if (i == GetAbilityID()) {
                EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Press + i, AttemptUseAbility);
            } else {
                EventManager.StopListening(owner.GetEntityID(), GameEvents.Ability_Press + i, OtherAbilityPressed);
            }
        }
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        owner.GetAttributes().RegisterListener(GetCooldownAttribute(), CooldownAttributeChanged);
        CooldownAttributeChanged((int) (owner.GetAttribute(GetCooldownAttribute()).GetValue() * 1000));
    }

    protected override bool CanUseAbility() {
        if (canBypassCooldown())
            return true;

        if (owner.GetEntityData(EntityDataType.Ability1_Cooldown + (int) GetAbilityID()) == 0) {
            ResourceCost resourceCost = GetResourceCost();
            EntityDataType resourceType = resourceCost.GetResourceType();

            return owner.GetEntityData(resourceType) >= resourceCost.GetCost(owner);
        }

        return false;
    }
    protected override void UseAbility() {
        base.UseAbility();

        if (!isCasting) {
            isCasting = true;
            ShowSpellIndicator();
        } else {
            isCasting = false;
            owner.GetSpellIndicators().ResetIndicators();

            CastAbility();
        }
    }

    protected virtual void CastAbility() {
        owner.SetEntityData(EntityDataType.Ability1_Cooldown + (int) GetAbilityID(), owner.GetAttribute(GetCooldownAttribute()).GetValue());
        owner.UseResource(GetResourceCost());
    }

    public virtual void CooldownAttributeChanged(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Cooldown_Max_Update + GetAbilityID(), param);
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

    void OtherAbilityPressed(int param) {
        isCasting = false;
    }

    public virtual void Update() {
        float cooldown = owner.GetEntityData(EntityDataType.Ability1_Cooldown + GetAbilityID());
        if (cooldown > 0) {
            cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);
            owner.SetEntityData(EntityDataType.Ability1_Cooldown + GetAbilityID(), cooldown);

            int percent = (int) (cooldown * 1000);
            EventManager.TriggerEvent(owner.GetEntityID(), GameEvents.Ability_Cooldown_Update + GetAbilityID(), percent);
        }
    }

    AttributeTypes GetCooldownAttribute () {
        return AttributeTypes.Ability1Cooldown + (int) GetAbilityID();
    }

    protected abstract List<Entity> GetEntitiesHitByAbility(int layerMask);
}
