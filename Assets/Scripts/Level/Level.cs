using UnityEngine;

public class Level : MonoBehaviour {
    PlayerSpawner[] playerSpawners;
    MobSpawner[] mobSpawners;
    WorldEventSystem WorldEvents;

    //  TODO: [Rock]: Create an entity manager, this class will handle the updating of entities

    private void Start() {
        playerSpawners = FindObjectsOfType<PlayerSpawner>();
        mobSpawners = FindObjectsOfType<MobSpawner>();

        WorldEvents = new WorldEventSystem(this);
    }
}
