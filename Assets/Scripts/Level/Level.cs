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

    private void Awake() {
        entityManager = new EntityManager(this);
        gameplayNodes = new GameplayNodeManager();
        gamemode = new HeroDefenseGamemode(this);
    }

    private void Start() {
        gameplayNodes.Setup();
        gamemode.Setup();
    }

    private void OnDisable() {
        gamemode.Breakdown();
    }

    //  Creates an Entity GameObject prefab, calls setup, and registers it
    public void SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) => entityManager.SpawnEntity(prefab, position, rotation);

    //  This is only called for Entities that aren't created via the SpawnEntity
    public void RegisterEntity(Entity entity) => entityManager.RegisterEntity(entity);

    private void Update() {
        gamemode.Update();
        entityManager.Update();
    }
}
