using RockUtils.GameEvents;
using UnityEngine;

public class CastAbilityBase : AbilityBase {
    protected float cooldown = 0;
    protected float maxCooldown;
    protected int manaCost = 10;

    public CastAbilityBase(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() { return "CastAbilityBase"; }

    protected override TriggerType GetTriggerType() { return TriggerType.Cast; }

    protected float GetCooldownTime() { return cooldown; }

    protected override void RegisterEvents() {
        base.RegisterEvents();

        EventManager.StartListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void UnregisterEvents() {
        base.UnregisterEvents();

        EventManager.StopListening(owner.GetEntityID(), (int) GameEvents.Ability_Press + (int) abilityID, AttemptUseAbility);
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        owner.GetAttributes().RegisterListener(GetCooldownAttribute().GetAttribute(), CooldownAttributeChanged);
        CooldownAttributeChanged((int) (GetCooldownAttribute().GetValue() * 1000));
    }

    protected override bool CanUseAbility() {
        return (cooldown == 0 && owner.GetMana() >= manaCost) || canBypassCooldown();
    }
    protected override void UseAbility() {
        base.UseAbility();

        cooldown = owner.GetAttribute(Player.ABILITY_COOLDOWNS[(int) abilityID]).GetValue();
        owner.UseMana(manaCost);
    }

    public virtual void CooldownAttributeChanged(int param) {
        maxCooldown = param / 1000f;
        EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Cooldown_Max_Update + (int) abilityID, param);
    }

    public virtual void Update() {
        //  TODO: [Rock]: We should not be updating the cooldown's value in the Ability. Investigate into allowing the cooldown stat to update itself
        if (cooldown > 0) {
            cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);

            int percent = (int) (cooldown * 1000);
            EventManager.TriggerEvent(owner.GetEntityID(), (int) GameEvents.Ability_Cooldown_Update + (int) abilityID, percent);
        }
    }

    AttributeInstance GetCooldownAttribute () {
        return owner.GetAttribute(Player.ABILITY_COOLDOWNS[(int) abilityID]);
    }
}
