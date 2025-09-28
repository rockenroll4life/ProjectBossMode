using UnityEngine;

public class Level : MonoBehaviour {
    IGamemode gamemode;
    EntityManager entityManager;
    GameplayNodeManager gameplayNodes;

    //  TODO: [Rock]: We should make some type of data struct for holding important game objects for a given gamemode, probably a Scriptable Object
    public GameObject characterPrefab;
    public GameObject towerPrefab;
    public GameObject mobSpawnerPrefab;

    public EntityManager EntityManager { get { return entityManager; } }
    public IGamemode Gamemode { get { return gamemode; } }
    public WorldEventSystem WorldEvents { get { return gamemode.GetWorldEvents(); } }
    public GameplayNodeManager GameplayNodes { get { return gameplayNodes; } }

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

    //  TODO: [Rock]: We needs to pass 2 different points of data
    //  1. EntityData (This contains what entity it is, and any important info about it)
    //  2. PositionalData (This contains position and rotation)
    //  Creates an Entity GameObject prefab, calls setup, and registers it
    public void SpawnEntity(GameObject prefab, Vector3 position, Quaternion rotation) => entityManager.SpawnEntity(prefab, position, rotation);

    //  NOTE: [Rock]: Good chance we don't need this
    //  This is only called for Entities that aren't created via the SpawnEntity
    //  but I think we should be creating all entities via the SpawnEntity
    public void RegisterEntity(Entity entity) => entityManager.RegisterEntity(entity);

    private void Update() {
        gamemode.Update();
        entityManager.Update();
    }
}
