using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugRotateTowardsObject : MonoBehaviour {
    public GameObject target = null;

    // Update is called once per frame
    void Update() {
        if (target != null) {
            gameObject.transform.LookAt(target.transform);
        }
    }
}
