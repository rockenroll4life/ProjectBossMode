public abstract class WorldEventBase : IWorldEvent {
    readonly Level level;

    public virtual bool TriggersOnStart() => false;

    public WorldEventBase(Level level) {
        this.level = level;
    }

    public abstract void TriggerEvent();

    
}
