using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour {
    //  TODO: [Rock]: Could be problematic...This will trigger the events for EVERYONE who subscribes to it, so if a mobs health changes the player will get a notification...HMMMM
    public readonly Stat HEALTH = new Stat("stat.generic.health", 100, 0, float.MaxValue, (int) GameEvents.Health_Changed);
    public readonly Stat MANA = new Stat("stat.generic.mana", 100, 0, float.MaxValue, (int) GameEvents.Mana_Changed);
    public readonly Stat MOVEMENT_SPEED = new Stat("stat.generic.movementSpeed", 13.2f, 0, float.MaxValue);

    //  TODO: We should make Unit Test that run this instead of clogging up the class file since Stat isn't a Monobehavior
    //  Debug test for stats to verify they're working correctly
    public void DebugTest() {
        Debug.Log("Begin EntityStats DebugTest");

        //  Debug Testing of stats
        Debug.Assert(HEALTH.GetValue() == 100, "INCORRECT VALUE! - Initial");

        System.Guid testGUID1 = new System.Guid("64b7f4ea-5ec8-44e9-bf9d-4415c7cb223d");
        System.Guid testGUID2 = new System.Guid("f0693954-2941-49bf-9c81-8aa1d74e818b");
        System.Guid testGUID3 = new System.Guid("8c9925c7-e103-4837-bbe7-86702aabe4af");
        System.Guid testGUID4 = new System.Guid("fdefeb26-302e-434c-8ed2-af18dd1e34be");

        //  Test 1 - Additive
        HEALTH.AddModifier(testGUID1, new StatModifier(StatModifier.ModifierType.Additive, 20));
        Debug.Assert(HEALTH.GetValue() == 120, "INCORRECT VALUE! - Test 1");

        //  Test 2 - 2nd Additive
        HEALTH.AddModifier(testGUID2, new StatModifier(StatModifier.ModifierType.Additive, 16));
        Debug.Assert(HEALTH.GetValue() == 136, "INCORRECT VALUE! - Test 2");

        //  Test 3: Override the first additive with a new value
        HEALTH.AddModifier(testGUID1, new StatModifier(StatModifier.ModifierType.Additive, 5));
        Debug.Assert(HEALTH.GetValue() == 121, "INCORRECT VALUE! - Test 3");

        //  Test 4: Multiplicative
        HEALTH.AddModifier(testGUID3, new StatModifier(StatModifier.ModifierType.Multiplicative, 0.75f));
        Debug.Assert(Mathf.Approximately(HEALTH.GetValue(), 211.75f), "INCORRECT VALUE! - Test 4");

        //  Test 5: 2nd Multiplicative
        HEALTH.AddModifier(testGUID4, new StatModifier(StatModifier.ModifierType.Multiplicative, 3.12f));
        Debug.Assert(Mathf.Approximately(HEALTH.GetValue(), 589.27f), "INCORRECT VALUE! - Test 4");

        Debug.Log("Finished EntityStats DebugTest");
    }
}