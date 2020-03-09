using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbacks : MonoBehaviour {
    Player player;
    void Start() {
        player = GetComponentInParent<Player>();
    }

    public void StartRoll() {
        player.StartRoll();
    }

    public void FinishRoll() {
        player.FinishRoll();
    }
}
