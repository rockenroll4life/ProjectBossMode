using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroDefenseGamemode : GamemodeBase {
    public HeroDefenseGamemode(Level level)
        : base(level) {
    }

    public override void Setup() {
        Level level = GetLevel();

        //  TODO: [Rock]: Eventually this will be multiplayer and we'll have to get this data from somewhere else...not sure where at the moment...
        level.SpawnEntity(level.characterPrefab, Vector3.zero, Quaternion.identity);
        level.SpawnEntity(level.towerPrefab, new Vector3(0, 1.73f, 4), Quaternion.identity);

        level.SpawnEntity(level.mobSpawnerPrefab, new Vector3(-5, 0, 20), Quaternion.identity);
        level.SpawnEntity(level.mobSpawnerPrefab, new Vector3(5, 0, 20), Quaternion.identity);

        GetWorldEvents().onEntityKilled += EntityDestroyed;
    }

    public override void Breakdown() {
        GetWorldEvents().onEntityKilled -= EntityDestroyed;
    }

    void EntityDestroyed(Entity entity) {
        if (entity is MobSpawner) {
            IReadOnlyList<Entity> spawners = GetLevel().GetEntityManager().GetEntities(typeof(MobSpawner));
            if (spawners.Count == 0) {
                Debug.Log("All spawners destroyed!");
            }
        }
    }
}
