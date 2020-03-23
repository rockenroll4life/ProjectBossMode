using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    static readonly int NUM_ABILITIES = 5;

    Entity owner;

    //
    public Ability[] abilities = new Ability[NUM_ABILITIES];

    public void Setup(Entity owner) {
        this.owner = owner;

        for (int i = 0; i < NUM_ABILITIES; i++) {
            if (i == 0) {
                abilities[i] = gameObject.AddComponent<TestNoStopAbility>();
            } else {
                abilities[i] = gameObject.AddComponent<TestAbility>();
            }
            abilities[i].Setup(owner, "Ability " + i, 10);
            abilities[i].SetAbilityID(i);
        }
    }
}
