using UnityEngine;

//  NOTE: [Rock] Primarily used for Testing purposes
public class PlayerSpawner : MonoBehaviour {
    public GameObject characterPrefab;

    void Start() {
        Debug.Assert(characterPrefab, "NO CHARACTER SET ON PLAYER SPAWNER!");
    }   

    private void Update() {
        Level level = FindObjectOfType<Level>();
        Debug.Assert(level, "LEVEL NO FOUND IN PLAYER SPAWNER!");

        level.SpawnEntity(characterPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Player");
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
    }
}
