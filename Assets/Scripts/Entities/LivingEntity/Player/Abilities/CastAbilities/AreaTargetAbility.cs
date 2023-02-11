public abstract class AreaTargetAbility : CastAbilityBase {
    protected AreaTargetAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected abstract float GetSpellRadius();

    protected abstract float GetRangeRadius();

    protected override void ShowSpellIndicator() {
        owner.GetSpellIndicators().AreaTarget(SpellIndicators.DEFAULT_COLOR, SpellIndicators.DEFAULT_COLOR, GetSpellRadius(), GetRangeRadius());
    }
}
