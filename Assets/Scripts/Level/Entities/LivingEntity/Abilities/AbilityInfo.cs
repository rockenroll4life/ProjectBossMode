using System.Collections.Generic;

namespace Ability {
    public enum ID {
        BurningPassion,
        FireBreath,
        RingOfFire,
        BurningKnowledge,
        Meteor,
    }

    public class Info {
        public static readonly ResourceCost FREE_RESOURCE_COST = new ResourceCost(null, EntityDataType.Mana, 0);
        public static int NUM_ABILITIES = 5;

        private static readonly Dictionary<ID, System.Type> abilities = new Dictionary<ID, System.Type>() {
            { ID.BurningPassion,    typeof(TestNoStopAbility) },
            { ID.FireBreath,        typeof(TestConeAbility) },
            { ID.RingOfFire,        typeof(TestAOEAbility) },
            { ID.BurningKnowledge,  typeof(TestChannelAbility) },
            { ID.Meteor,            typeof(TestAreaTargetAbility) },
        };

        public static System.Type GetAbilityClass(ID abilityID) => abilities[abilityID];
    }

    public enum Binding {
        NONE = -1,
        Ability1,
        Ability2,
        Ability3,
        Ability4,
        Ultimate,
    }

}