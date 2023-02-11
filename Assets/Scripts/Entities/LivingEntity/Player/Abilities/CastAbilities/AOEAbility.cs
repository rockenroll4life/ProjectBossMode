public abstract class AOEAbility : CastAbilityBase {
    public AOEAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected abstract float GetSpellRadius();

    protected override void ShowSpellIndicator() {
        owner.GetSpellIndicators().AOE(SpellIndicators.DEFAULT_COLOR, GetSpellRadius());
    }
}
