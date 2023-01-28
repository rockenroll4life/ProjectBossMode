using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Entity {
    protected override void Initialize() {
        base.Initialize();

        entityType = EntityType.Mob;
        highlightColor = Color.red;
    }

    protected virtual void AIStep() { }

    protected override void UpdateStep() {
        base.UpdateStep();

        //  Handle any AI stuffs or extra handling this entity needs
        AIStep();
    }
}
