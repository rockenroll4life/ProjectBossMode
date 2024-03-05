using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoveToFromObject : MonoBehaviour {
    public GameObject target = null;

    public float distance;
    public Vector3 direction;
    bool isSetup = false;

    void Update() {
        if (target != null) {
            if (!isSetup) {
                Setup();
            }

            transform.position = (target.transform.position + (direction * distance));
        } else {
            isSetup = false;
        }
    }

    void Setup() {
        isSetup = true;

        direction = (target.transform.position - transform.position).normalized;
        distance = Vector3.Distance(target.transform.position, transform.position);
    }
}
