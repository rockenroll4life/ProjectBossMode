using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Entity {
    protected override void Initialize() {
        base.Initialize();

        entityType = EntityType.Interactable;
        highlightColor = Color.white;
    }
}
