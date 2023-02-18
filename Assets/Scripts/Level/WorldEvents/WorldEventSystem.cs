using System;
using System.Collections.Generic;

public class WorldEventSystem {
    //  World Event Delegates, world events can listen to this
    readonly public Action<Entity> onEntitySpawned;
    readonly public Action<Entity> onEntityKilled;

    readonly List<IWorldEvent> worldEvents = new();
    readonly Level level;

    public WorldEventSystem(Level level) {
        this.level = level;
    }
}
