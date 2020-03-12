using UnityEngine;

public class FixedFollowObject : MonoBehaviour {
    public GameObject target;
    Vector3 offset;

    private void Start() {
        offset = target.transform.position - transform.position;
    }

    private void LateUpdate() {
        if (target != null) {
            transform.position = target.transform.position - offset;
        }
    }
}
