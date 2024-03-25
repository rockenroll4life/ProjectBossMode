using UnityEngine;
using UnityEngine.VFX;

public class AbilityParticle : MonoBehaviour {
    VisualEffect effect;
    bool isActive = false;

    void Start() {
        effect = GetComponent<VisualEffect>();
    }

    private void Update() {
        //  Note: [Rock]: I hate this so much...but seems to be the only way to really check...
        if (!isActive) {
            if (effect.aliveParticleCount > 0) {
                isActive = true;
            }
        } else {
            if (effect.aliveParticleCount <= 0) {
                Destroy(gameObject);
            }
        }
    }
}
