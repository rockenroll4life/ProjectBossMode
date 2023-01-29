using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Entity {
    public override EntityType GetEntityType() { return EntityType.Mob; }
    public override TargetingManager.TargetType GetTargetType() { return TargetingManager.TargetType.Mob; }

    protected override Color? GetHighlightColor() { return Color.red; }
    protected override Color? GetHighlightOutlineColor() { return Color.red; }

    protected virtual void AIStep() { }

    protected override void UpdateStep() {
        base.UpdateStep();

        //  Handle any AI stuffs or extra handling this entity needs
        AIStep();
    }
}
