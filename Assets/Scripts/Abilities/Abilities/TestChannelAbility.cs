using UnityEngine;

public class TestChannelAbility : ChannelAbilityBase {
    protected override string GetName() { return "ChannelAbilityBase"; }

    protected override void UseAbility() {
        base.UseAbility();

        Debug.Log("Channeling!");
    }
}
