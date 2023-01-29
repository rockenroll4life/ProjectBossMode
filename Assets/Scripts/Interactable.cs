using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Entity {
    protected override void Initialize() {
        base.Initialize();

        entityType = EntityType.Interactable;
    }

    public override TargetingManager.TargetType GetTargetType() { return TargetingManager.TargetType.Interactable; }
}
