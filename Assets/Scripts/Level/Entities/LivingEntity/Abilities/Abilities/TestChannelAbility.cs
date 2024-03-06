public class TestChannelAbility : ChannelAbilityBase {
    public TestChannelAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
        cost = new ResourceCost(EntityDataType.Health, 10);
    }

    protected override string GetName() { return "ChannelAbilityBase"; }
}
