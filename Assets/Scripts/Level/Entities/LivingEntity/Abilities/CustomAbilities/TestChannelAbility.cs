public class TestChannelAbility : ChannelAbilityBase {
    public TestChannelAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
    }

    protected override string GetName() { return "ChannelAbilityBase"; }
    public override Ability.ID GetID() => Ability.ID.BurningKnowledge;
}
