using System.Collections.Generic;
using UnityEngine;

public class EntityManager {
    readonly Level level;

    readonly List<Entity> entities = new();
    readonly List<Entity> toRemove = new();
    readonly List<Entity> toAdd = new();

    public EntityManager(Level level) {
        this.level = level;
    }

    public void Update() {
        //  Add any new entities
        entities.AddRange(toAdd);
        toAdd.Clear();

        //  Update the entities
        entities.ForEach(entity =>{
            entity.Tick();

            if (entity.IsDead()) {
                toRemove.Add(entity);
            }
        });

        //  Get rid of dead enemies
        toRemove.ForEach(entity => {
            entity.Breakdown();
            entities.Remove(entity);
            Object.Destroy(entity.gameObject);
        });

        toRemove.Clear();
    }

    public void RegisterEntity(Entity entity) {
        if (entity) {
            toAdd.Add(entity);
        }
    }

    public void RegisterEntities(List<Entity> entities) {
        entities.ForEach(entity => RegisterEntity(entity));
    }
}
