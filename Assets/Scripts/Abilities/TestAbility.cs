using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : Ability {
    public override void Setup(Entity owner, string name, float cooldownTime) {
        base.Setup(owner, name, cooldownTime);

        interruptsMovement = true;
    }
}
