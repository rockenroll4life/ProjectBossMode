using UnityEngine;

public class EntityAnimator : MonoBehaviour {
    public GameObject testSwordForAttaching;

    protected Entity owner;
    protected Animator animator;

    public void SetOwner(Entity owner) {
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
