using UnityEngine;

public class Level : MonoBehaviour {
    IGamemode gamemode;
    EntityManager entityManager;
    GameplayNodeManager gameplayNodes;

    //  TODO: [Rock]: We should make some type of data struct for holding important game objects for a given gamemode, probably a Scriptable Object
    public GameObject characterPrefab;
    public GameObject towerPrefab;
    public GameObject mobSpawnerPrefab;

    //  NOTE: [Rock]: We can probably change these getters into properties
    public EntityManager GetEntityManager() => entityManager;
    public IGamemode GetGamemode() => gamemode;
    public WorldEventSystem GetWorldEvents() => gamemode.GetWorldEvents();
    public GameplayNodeManager GetGameplayNodes() => gameplayNodes;

    private void Start() {
        entityManager = new EntityManager();

        gameplayNodes = new GameplayNodeManager();
        gameplayNodes.Setup();

        gamemode = new HeroDefenseGamemode(this);
        gamemode.Setup();
    }

    private void OnDisable() {
        gamemode.Breakdown();
    }

    //  NOTE: [Rock]: We may want to move the spawning of entities into the EntityManager...hmmmm

    //  Creates an Entity GameObject prefab, calls setup, and registers it
    public void SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) {
        if (prefab == null) {
            Debug.LogError("FAILED TO SPAWN ENTITY DUE TO NULL PREFAB!");
            return;
        }

        GameObject obj = Instantiate(prefab, position, rotation);
        Entity entity = obj.GetComponent<Entity>();

        Debug.Assert(entity, "INVALID SPAWN ENTITY PREFAB");

        entity.Setup(this);
        RegisterEntity(entity);
    }

    //  This is only called for Entities that aren't created via the SpawnEntity
    public void RegisterEntity(Entity entity) {
        entityManager.RegisterEntity(entity);
    }

    private void Update() {
        entityManager.Update();
    }
}
