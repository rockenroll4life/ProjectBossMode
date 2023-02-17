public class Interactable : Entity {
    public override EntityType GetEntityType() => EntityType.Interactable;

    public override bool IsDead() => false;
}
