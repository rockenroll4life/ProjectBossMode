using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : Locomotion {
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        agent.updateRotation = false;
    }
}
