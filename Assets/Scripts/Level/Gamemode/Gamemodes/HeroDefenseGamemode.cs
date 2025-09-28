using RockUtils.GameEvents;
using System.Collections.Generic;
using UnityEngine;

public class HeroDefenseGamemode : GamemodeBase {
    //  TEMP
    private bool hasSpawned = false;

    public HeroDefenseGamemode(Level level)
        : base(level) {
    }

    public override void Setup() {
        Level level = GetLevel();

        //  TODO: [Rock]: Eventually this will be multiplayer and we'll have to get this data from somewhere else...not sure where at the moment...
        PopulateNodes(GameplayNode.Type.PlayerSpawn, level.characterPrefab, Vector3.zero);

        PopulateNodes(GameplayNode.Type.Tower, level.towerPrefab, new Vector3(0, 1.73f, 4));
        PopulateNodes(GameplayNode.Type.MobSpawner, level.mobSpawnerPrefab, Vector3.zero);

        GetWorldEvents().onEntityKilled += EntityDestroyed;
    }

    public override void Breakdown() {
        GetWorldEvents().onEntityKilled -= EntityDestroyed;
    }

    public override void Update() {
        if (!hasSpawned) {
            hasSpawned = true;
            EventManager.TriggerEvent(GameEvents.MobSpawner_SpawnEntities);
        }
    }

    void EntityDestroyed(Entity entity) {
        if (entity is MobSpawner) {
            IReadOnlyList<Entity> spawners = GetLevel().EntityManager.GetEntities(typeof(MobSpawner));
            if (spawners.Count == 0) {
                Debug.Log("All spawners destroyed!");
            }
        }
    }
}
