using UnityEngine;

public class TestCharacter1 : Player {
    protected override void RegisterAbilities() {
        abilities.SetAbility(AbilityNum.Ability1, typeof(TestNoStopAbility));
        abilities.SetAbility(AbilityNum.Ability2, typeof(TestConeAbility));
        abilities.SetAbility(AbilityNum.Ability3, typeof(TestAOEAbility));
        abilities.SetAbility(AbilityNum.Ability4, typeof(TestAreaTargetAbility));
        abilities.SetAbility(AbilityNum.Ultimate, typeof(TestChannelAbility));
    }
}
