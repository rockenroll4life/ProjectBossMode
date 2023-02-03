using UnityEngine;

public class EntityAnimator {
    public GameObject testSwordForAttaching;

    protected LivingEntity owner;
    protected Animator animator;

    public EntityAnimator(LivingEntity owner) {
        this.owner = owner;
        animator = owner.GetComponentInChildren<Animator>();
    }
    
    public virtual void Update() { }
}
