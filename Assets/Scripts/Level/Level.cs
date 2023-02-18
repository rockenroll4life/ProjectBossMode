using UnityEngine;
using System.Linq;
using RockUtils.GameEvents;

public class Level : MonoBehaviour {
    EntityManager entities;
    WorldEventSystem worldEvents;

    public EntityManager GetEntityManager() => entities;
    public WorldEventSystem GetWorldEvents() => worldEvents;

    private void Start() {
        worldEvents = new WorldEventSystem(this);
        entities = new EntityManager();

        //  TODO: [Rock]: We'll want to setup the player spawning in the level and not in the player spawner. We probably don't even need it really...
        //List<PlayerSpawner> playerSpawners = FindObjectsOfType<PlayerSpawner>().ToList();

        //  Register Entities
        Tower tower = FindObjectOfType<Tower>();
        tower.Setup(this);

        entities.RegisterEntity(tower);
        entities.RegisterEntities(FindObjectsOfType<MobSpawner>().Cast<Entity>().ToList());
    }

    //  NOTE: [Rock]: We may want to move the spawning of entities into the EntityManager...hmmmm

    //  Creates an Entity GameObject prefab, calls setup, and registers it
    public void SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject obj = Instantiate(prefab, position, rotation);
        Entity entity = obj.GetComponent<Entity>();

        Debug.Assert(entity, "INVALID SPAWN ENTITY PREFAB");

        entity.Setup(this);
        RegisterEntity(entity);
    }

    //  This is only called for Entities that aren't created via the SpawnEntity
    public void RegisterEntity(Entity entity) {
        entities.RegisterEntity(entity);
    }

    private void Update() {
        entities.Update();
    }
}
