public class TestCharacter1 : Player {
    protected override void RegisterAbilities() {
        GetAbilities().RegisterAbility(new TestNoStopAbility(this, AbilityNum.Ability1));
        GetAbilities().RegisterAbility(new TestConeAbility(this, AbilityNum.Ability2));
        GetAbilities().RegisterAbility(new TestAOEAbility(this, AbilityNum.Ability3));
        GetAbilities().RegisterAbility(new TestAreaTargetAbility(this, AbilityNum.Ability4));
        GetAbilities().RegisterAbility(new TestChannelAbility(this, AbilityNum.Ultimate));
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        //  Update any of the base attribute values for this character
        GetAttribute(Attributes.Get(AttributeTypes.HealthMax)).SetBaseValue(500);
        SetResource(ResourceType.Health, GetAttribute(Attributes.Get(AttributeTypes.HealthMax)).GetValue());

        GetAttribute(Attributes.Get(AttributeTypes.ManaMax)).SetBaseValue(100);
        SetResource(ResourceType.Mana, GetAttribute(Attributes.Get(AttributeTypes.ManaMax)).GetValue());
    }
}
