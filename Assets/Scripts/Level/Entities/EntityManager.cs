using System.Collections.Generic;
using UnityEngine;

public class EntityManager {
    private static readonly List<Entity> NO_ENTITIES = new();

    private readonly List<Entity> entities = new();
    //  NOTE: [Rock]: We may want to support storing an entity in multiple types in the future
    private readonly Dictionary<System.Type, List<Entity>> entityDictionary = new();

    private readonly List<Entity> toRemove = new();
    private readonly List<Entity> toAdd = new();

    private readonly Level level;

    public EntityManager(Level level) {
        this.level = level;
    }

    public void Update() {
        //  Add any new entities
        AddNewEntities();
 
        //  Update the entities
        entities.ForEach(entity =>{
            entity.Tick();

            if (entity.IsDead()) {
                toRemove.Add(entity);
            }
        });

        //  Get rid of dead enemies
        RemoveDeadEntities();
    }

    void AddNewEntities() {
        toAdd.ForEach(entity => {
            entities.Add(entity);

            System.Type type = entity.GetSystemType();
            if (entityDictionary.ContainsKey(type)) {
                entityDictionary[type].Add(entity);
            } else {
                List<Entity> newList = new();
                newList.Add(entity);
                entityDictionary.Add(type, newList);
            }
        });

        toAdd.Clear();
    }

    void RemoveDeadEntities() {
        toRemove.ForEach(entity => {
            entities.Remove(entity);
            entityDictionary[entity.GetSystemType()].Remove(entity);

            entity.Breakdown();
            Object.Destroy(entity.gameObject);
        });

        toRemove.Clear();
    }

    public void SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
        if (prefab == null) {
            Debug.LogError("FAILED TO SPAWN ENTITY DUE TO NULL PREFAB!");
            return;
        }

        GameObject obj = Object.Instantiate(prefab, position, rotation);
        Entity entity = obj.GetComponent<Entity>();

        Debug.Assert(entity, "INVALID SPAWN ENTITY PREFAB");

        entity.Setup(level);
        RegisterEntity(entity);
    }

    public void RegisterEntity(Entity entity) {
        if (entity) {
            toAdd.Add(entity);
        }
    }

    public void RegisterEntities(List<Entity> entities) {
        entities.ForEach(entity => RegisterEntity(entity));
    }

    public IReadOnlyList<Entity> GetAllEntities() {
        return entities;
    }

    public IReadOnlyList<Entity> GetEntities(System.Type type) {
        if (type == typeof(Entity)) {
            return entities;
        } else if (entityDictionary.ContainsKey(type)) {
            return entityDictionary[type];
        }

        return NO_ENTITIES;
    }

    public T GetFirstEntityOfType<T>() where T : Entity {
        if (entityDictionary.ContainsKey(typeof(T)) && entityDictionary[typeof(T)].Count > 0) {
            return (T) entityDictionary[typeof(T)][0];
        }

        return null;
    }
}
