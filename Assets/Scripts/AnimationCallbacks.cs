using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbacks : MonoBehaviour {
    public Player player;

    public void OnStartRoll_AnimationEvent() {
        player.OnStartRoll_AnimationEvent();
    }

    public void OnFinishRoll_AnimationEvent() {
        player.OnFinishRoll_AnimationEvent();
    }
}
