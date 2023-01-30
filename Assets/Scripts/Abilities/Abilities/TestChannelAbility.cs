using UnityEngine;

public class TestChannelAbility : ChannelAbilityBase {
    protected override string GetName() { return "ChannelAbilityBase"; }

    protected override void UseAbility() {
        base.UseAbility();

        
    }

    protected override void AbilityStart(int param) {
        base.AbilityStart(param);

        Debug.Log("Start Channeling!");
    }

    protected override void AbilityStop(int param) {
        base.AbilityStop(param);

        Debug.Log("Stop Channeling!");
    }
}
