public abstract class ConeAbility : CastAbilityBase {
    public ConeAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected abstract float GetSpellRadius();

    protected abstract float GetSpellAngle();

    protected override void ShowSpellIndicator() {
        owner.GetSpellIndicators().Cone(SpellIndicators.DEFAULT_COLOR, GetSpellRadius(), GetSpellAngle());
    }
}
