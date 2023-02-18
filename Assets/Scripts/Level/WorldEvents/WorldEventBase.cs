public abstract class WorldEventBase : IWorldEvent {
    readonly WorldEventSystem worldEventSystem;

    public WorldEventBase(WorldEventSystem worldEventSystem) {
        this.worldEventSystem = worldEventSystem;
    }

    public abstract void TriggerEvent();
}
