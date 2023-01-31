using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Entity {
    public override EntityType GetEntityType() { return EntityType.Interactable; }
    public override TargetingManager.TargetType GetTargetType() { return TargetingManager.TargetType.Interactable; }
}
