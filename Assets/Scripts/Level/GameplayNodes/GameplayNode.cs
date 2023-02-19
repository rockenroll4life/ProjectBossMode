using UnityEngine;

public class GameplayNode : MonoBehaviour {
    public enum Type {
        Node,
        PlayerSpawn,
        MobSpawner,
        Tower,
    }

    public Type type;

    private void OnDrawGizmos() {
        switch (type) {
            case Type.PlayerSpawn: {
                    Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Player");
                    Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
                    break;
                }
            case Type.MobSpawner: {
                    Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Mob");
                    Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Spawner");
                    break;
                }
            case Type.Tower: {
                    Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Tower");
                    break;
                }
            default:
                Gizmos.DrawIcon(transform.position + Vector3.up * 2, "Node");
                break;
        }
    }
}
