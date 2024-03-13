public class TestChannelAbility : ChannelAbilityBase {
    public TestChannelAbility(Player owner, Ability.Binding abilityBinding)
        : base(owner, abilityBinding) {
        cost = new ResourceCost(owner, EntityDataType.Health, 10);
    }

    protected override string GetName() { return "ChannelAbilityBase"; }
    public override Ability.ID GetID() => Ability.ID.BurningKnowledge;
}
