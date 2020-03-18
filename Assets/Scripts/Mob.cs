using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Entity {
    protected override void Initialize() {
        base.Initialize();

        entityType = EntityType.Mob;
        highlightColor = Color.red;
    }
}
