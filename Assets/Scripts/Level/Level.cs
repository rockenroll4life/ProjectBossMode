using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    EntityManager entities;
    WorldEventSystem worldEvents;

    //  TODO: [Rock]: Create an entity manager, this class will handle the updating of entities

    public EntityManager GetEntityManager() => entities;

    private void Start() {
        worldEvents = new WorldEventSystem(this);
        entities = new EntityManager(this);

        List<PlayerSpawner> playerSpawners = FindObjectsOfType<PlayerSpawner>().ToList();

        //  Register Entities
        entities.RegisterEntity(FindObjectOfType<Tower>());
        entities.RegisterEntities(FindObjectsOfType<MobSpawner>().Cast<Entity>().ToList());
    }

    public void SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject obj = Instantiate(prefab, position, rotation);
        Entity entity = obj.GetComponent<Entity>();

        Debug.Assert(entity, "INVALID SPAWN ENTITY PREFAB");

        entity.Setup(this);
        RegisterEntity(entity);
    }

    public void RegisterEntity(Entity entity) {
        entities.RegisterEntity(entity);
    }

    private void Update() {
        entities.Update();
    }
}
