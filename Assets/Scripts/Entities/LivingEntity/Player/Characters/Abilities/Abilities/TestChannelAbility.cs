using UnityEngine;

public class TestChannelAbility : ChannelAbilityBase {
    public TestChannelAbility(Player owner, AbilityNum abilityNum)
        : base(owner, abilityNum) {
    }

    protected override string GetName() { return "ChannelAbilityBase"; }

    protected override void UseAbility() {
        base.UseAbility();

    }

    protected override void AbilityStart(int param) {
        base.AbilityStart(param);

        
    }

    protected override void AbilityStop(int param) {
        base.AbilityStop(param);

        
    }
}
