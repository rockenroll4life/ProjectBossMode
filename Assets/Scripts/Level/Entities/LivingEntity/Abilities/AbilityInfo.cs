using System.Collections.Generic;

namespace Ability {

    public class Info {
        public static int NUM_ABILITIES = 5;

        private static readonly Dictionary<ID, System.Type> abilities = new Dictionary<ID, System.Type>() {
            { ID.BurningPassion,    typeof(TestNoStopAbility) },
            { ID.FireBreath,        typeof(TestConeAbility) },
            { ID.RingOfFire,        typeof(TestAOEAbility) },
            { ID.BurningKnowledge,  typeof(TestChannelAbility) },
            { ID.Meteor,            typeof(TestAreaTargetAbility) },
        };

        public static System.Type GetAbility(ID abilityID) => abilities[abilityID];
    }

    public enum Binding {
        NONE = -1,
        Ability1,
        Ability2,
        Ability3,
        Ability4,
        Ultimate,
    }

    public enum ID {
        BurningPassion,
        FireBreath,
        RingOfFire,
        BurningKnowledge,
        Meteor,
    }
}