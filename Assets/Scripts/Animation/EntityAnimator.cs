using UnityEngine;

public class EntityAnimator : MonoBehaviour {
    public GameObject testSwordForAttaching;

    protected LivingEntity owner;
    protected Animator animator;

    public void SetOwner(LivingEntity owner) {
        this.owner = owner;
    }

    void Start() {
        animator = GetComponentInChildren<Animator>();
    }
    
    void Update() {
        UpdateAnimations();
    }

    protected virtual void UpdateAnimations() { }
}
