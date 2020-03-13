using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    static readonly int NUM_ABILITIES = 5;

    //
    public Ability[] abilities = new Ability[NUM_ABILITIES];

    public void Setup() {
        for (int i = 0; i < NUM_ABILITIES; i++) {
            abilities[i] = gameObject.AddComponent<TestAbility>();
            abilities[i].Setup("Ability " + i, 10);
            abilities[i].SetAbilityID(i);
        }
    }
}
