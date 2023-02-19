using RockUtils.ContainerUtils;
using System;
using System.Collections.Generic;

public class WorldEventSystem {
    //  World Event Delegates, world events can listen to this
    public Action<Entity> onEntitySpawned;
    public Action<Entity> onEntityKilled;

    readonly List<IWorldEvent> worldEvents = new();
    readonly List<KeyValuePair<IWorldEvent, int>> worldEventWeights = new(); 
    readonly Level level;

    float totalWeight = 0;

    public WorldEventSystem(Level level) {
        this.level = level;
    }

    public void RegisterWorldEvent(IWorldEvent worldEvent, int weight) {
        worldEvents.Add(worldEvent);

        if (worldEvent.TriggersOnStart()) {
            worldEvent.TriggerEvent();
        } else {
            worldEventWeights.Add(new KeyValuePair<IWorldEvent, int>(worldEvent, weight));
            totalWeight += weight;
        }
    }

    public void RegisterWorldEvent(IWorldEvent worldEvent) {
        RegisterWorldEvent(worldEvent, 0);
    }

    public void ClearWorldEvents() {
        worldEvents.Clear();
        worldEventWeights.Clear();
        totalWeight = 0;
    }

    public void TriggerEvent() {
        ContainerUtils.ShuffleList(worldEventWeights);

        //  NOTE: [Rock]: We may want to consider a different way of handling the weighting as one item in here will always trigger,
        //  but two might not trigger either if I'm reading it correctly
        foreach(KeyValuePair<IWorldEvent, int> pair in worldEventWeights) {
            float eventWeight = pair.Value / totalWeight;

            if (UnityEngine.Random.value <= eventWeight) {
                pair.Key.TriggerEvent();
                break;
            }
        }
    }
}
