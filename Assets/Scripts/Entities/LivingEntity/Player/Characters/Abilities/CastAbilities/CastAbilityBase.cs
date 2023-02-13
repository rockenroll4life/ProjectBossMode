using RockUtils.GameEvents;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastAbilityBase : AbilityBase {
    protected float cooldown = 0;
    bool isCasting = false;

    public CastAbilityBase(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override TriggerType GetTriggerType() => TriggerType.Cast;
    public override int GetManaCost() => 10;

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, MoveCancelAbility);
        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, CancelAbility);
        EventManager.StartListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.Escape, CancelAbility);

        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            if (i == GetAbilityID()) {
                EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + i, AttemptUseAbility);
            } else {
                EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + i, OtherAbilityPressed);
            }
        }
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_World, MoveCancelAbility);
        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Targeted_Entity, CancelAbility);
        EventManager.StopListening((int) GameEvents.KeyboardButton_Pressed + (int) KeyCode.Escape, CancelAbility);

        for (int i = 0; i < (int) AbilityNum._COUNT; i++) {
            if (i == GetAbilityID()) {
                EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + i, AttemptUseAbility);
            } else {
                EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + i, OtherAbilityPressed);
            }
        }
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        owner.GetAttributes().RegisterListener(GetCooldownAttribute().GetAttribute(), CooldownAttributeChanged);
        CooldownAttributeChanged((int) (GetCooldownAttribute().GetValue() * 1000));
    }

    protected override bool CanUseAbility() {
        return (cooldown == 0 && owner.GetMana() >= GetManaCost()) || canBypassCooldown();
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
        cooldown = owner.GetAttribute(Player.ABILITY_COOLDOWNS[GetAbilityID()]).GetValue();
        owner.UseMana(GetManaCost());
    }

    public virtual void CooldownAttributeChanged(int param) {
        EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Cooldown_Max_Update + GetAbilityID(), param);
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
        //  TODO: [Rock]: We should not be updating the cooldown value in the Ability. Investigate into allowing the cooldown stat to update itself
        if (cooldown > 0) {
            cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);

            int percent = (int) (cooldown * 1000);
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Cooldown_Update + GetAbilityID(), percent);
        }
    }

    AttributeInstance GetCooldownAttribute () {
        return owner.GetAttribute(Player.ABILITY_COOLDOWNS[GetAbilityID()]);
    }

    protected abstract List<Entity> GetEntitiesHitByAbility(int layerMask);
}
