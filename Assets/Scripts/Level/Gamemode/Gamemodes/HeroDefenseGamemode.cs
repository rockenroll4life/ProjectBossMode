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

        //  TODO: [Rock]: We should use some type of nodes in the level that we spawn the Mob Spawners at (Maybe Gameplay Nodes?)
        Object.FindObjectsOfType<MobSpawner>()
            .Cast<Entity>()
            .ToList()
            .ForEach(entity => {
                level.RegisterEntity(entity);
            });
    }

    public override void Breakdown() { }
}
