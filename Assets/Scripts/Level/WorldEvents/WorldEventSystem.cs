using System.Collections.Generic;

public class WorldEventSystem {
    readonly List<IWorldEvent> worldEvents = new();
    readonly Level level;

    public WorldEventSystem(Level level) {
        this.level = level;
    }
}
