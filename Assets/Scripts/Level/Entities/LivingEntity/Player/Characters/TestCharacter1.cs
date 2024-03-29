public class TestCharacter1 : Player {
    protected override void RegisterAbilities() {
        GetAbilities().RegisterAbility(Ability.ID.BurningPassion, Ability.Binding.Ability1);
        GetAbilities().RegisterAbility(Ability.ID.FireBreath, Ability.Binding.Ability2);
        GetAbilities().RegisterAbility(Ability.ID.RingOfFire, Ability.Binding.Ability3);
        GetAbilities().RegisterAbility(Ability.ID.BurningKnowledge, Ability.Binding.Ability4);
        GetAbilities().RegisterAbility(Ability.ID.Dash, Ability.Binding.Movement);
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

        //  Update any of the base attribute values for this character
        GetAttribute(AttributeTypes.HealthMax).SetBaseValue(500);
        SetEntityData(EntityDataType.Health, GetAttribute(AttributeTypes.HealthMax).GetValue());

        GetAttribute(AttributeTypes.ManaMax).SetBaseValue(100);
        SetEntityData(EntityDataType.Mana, GetAttribute(AttributeTypes.ManaMax).GetValue());
    }
}
