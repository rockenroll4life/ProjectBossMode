using System.Collections.Generic;

public class InstantCastAbility : CastAbilityBase {
    public InstantCastAbility(LivingEntity owner, Ability.ID abilityID, Ability.Binding abilityBinding)
        : base (owner, abilityID, abilityBinding) {
    }

    protected override bool BypassSpellIndicators() => true;
    protected override List<Entity> GetEntitiesHitByAbility(int layerMask) => new List<Entity>();

    protected override void ShowSpellIndicator() {
        //  Instant cast abilities don't show spell indicators
    }
}
