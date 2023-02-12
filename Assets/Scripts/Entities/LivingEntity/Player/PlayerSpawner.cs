using UnityEngine;

//  NOTE: [Rock] Primarily used for Testing purposes
public class PlayerSpawner : MonoBehaviour {
    public Player character;
    
    void Start() {
        Debug.Assert(character, "NO CHARACTER SET ON PLAYER SPAWNER!");

        Instantiate(character, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
