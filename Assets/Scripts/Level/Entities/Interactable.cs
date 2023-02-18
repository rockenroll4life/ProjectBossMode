using System;

public class Interactable : Entity {
    public override EntityType GetEntityType() => EntityType.Interactable;
    public override Type GetSystemType() => typeof(Interactable);

    public override bool IsDead() => false;
}
