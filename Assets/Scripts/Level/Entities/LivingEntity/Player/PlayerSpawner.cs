using UnityEngine;

//  NOTE: [Rock] Primarily used for Testing purposes
public class PlayerSpawner : MonoBehaviour {
    public Player character;
    
    void Start() {
        Debug.Assert(character, "NO CHARACTER SET ON PLAYER SPAWNER!");

        Instantiate(character, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Player");
        Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
    }
}
