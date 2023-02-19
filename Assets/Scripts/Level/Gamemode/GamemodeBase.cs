public abstract class GamemodeBase : IGamemode {
    readonly Level level;
    readonly WorldEventSystem worldEvents;

    protected Level GetLevel() => level;
    public WorldEventSystem GetWorldEvents() => worldEvents;

    public GamemodeBase(Level level) {
        this.level = level;
        worldEvents = new WorldEventSystem(level);
    }

    public abstract void Setup();
    public abstract void Breakdown();

    protected virtual void RegisterWorldEvents() { }
}
