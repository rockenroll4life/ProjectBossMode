using UnityEngine;

public class AbilityManager {
    static readonly int NUM_ABILITIES = 5;

    Entity owner;

    //
    public Ability[] abilities = new Ability[NUM_ABILITIES];

    public void Setup(Entity owner) {
        this.owner = owner;

        for (int i = 0; i < NUM_ABILITIES; i++) {
            if (i == 0) {
                abilities[i] = new TestNoStopAbility();
            } else {
                abilities[i] = new TestAbility();
            }
            //  TODO: [Rock]: We should never need to setup the ability name nor cooldown, this should be done via the ability
            abilities[i].Setup(owner, "Ability " + i, 10);
            abilities[i].SetAbilityID(i);
        }
    }

    public void Update() {
        foreach(Ability ability in abilities) {
            ability.Update();
        }
    }
}
