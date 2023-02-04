using UnityEngine;
using RockUtils.GameEvents;

public abstract class LivingEntity : Entity {
    public GameObject attackProjectilePrefab;
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    protected StatusEffectManager statusEffects;
    protected Locomotion locomotion;
    protected EntityAnimator animator;
    protected Targeter targeter;
    AttributeDictionary attributes;

    float attackTimer = 0;

    //  TODO: [Rock]: Replace with EntityData
    protected float health;

    public override EntityType GetEntityType() { return EntityType.LivingEntity; }

    public Targeter GetTargeter() { return targeter; }
    public Locomotion GetLocomotion() { return locomotion; }

    protected override void Setup() {
        base.Setup();

        RegisterEvents();
        RegisterAttributes();
        RegisterComponents();
    }

    protected override void Breakdown() {
        base.Breakdown();

        UnregisterEvents();
        UnregisterComponents();
    }

    protected virtual void RegisterComponents() {
        statusEffects = new StatusEffectManager(this);
    }

    protected virtual void UnregisterComponents() { }

    protected virtual void RegisterAttributes() {
        //  These are the base attributes that every entity has, only register attributes here that everyone will have (Even if we set them to a value
        //  of 0 in the actual entity themselves.
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MAX_HEALTH);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.ATTACK_DAMAGE);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.ATTACK_SPEED);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.ATTACK_RANGE);

        health = GetAttribute(LivingEntitySharedAttributes.MAX_HEALTH).GetValue();
    }

    //  TODO: [Rock]: We need support for entities to be able to say 'nah' to status effects and the applying fails
    public virtual void OnStatusEffectApplied(StatusEffect effect) { }

    public virtual void OnStatusEffectRemoved(StatusEffect effect) { }

    protected override void PreUpdateStep() {
        base.PreUpdateStep();

        statusEffects.Update();
        locomotion.Update();
        animator.Update();
    }

    protected override void UpdateStep() {
        base.UpdateStep();

        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        } else {
            if (CanAttack()) {
                Attack();
            }
        }
    }

    public AttributeDictionary GetAttributes() {
        if (attributes == null) {
            attributes = new AttributeDictionary(this);
        }

        return attributes;
    }

    public AttributeInstance GetAttribute(Attribute attribute) {
        return GetAttributes().GetInstance(attribute);
    }

    public float GetHealth() { return health; }

    protected virtual bool CanAttack() {
        return attackTimer <= 0 && targeter.GetTargetedEntity() != null;
    }

    protected virtual void Attack() {
        attackTimer = GetAttribute(LivingEntitySharedAttributes.ATTACK_SPEED).GetValue();

        Vector3 offset = (transform.forward * 1) + Vector3.up;
        Projectile proj = Instantiate(attackProjectilePrefab, transform.position + offset, transform.rotation).GetComponent<Projectile>();
        proj.Setup(this, targeter.GetTargetedEntity(), GetAttribute(LivingEntitySharedAttributes.ATTACK_DAMAGE).GetValue());
    }

    public virtual void Hurt(Entity damager, float damage) {
        health -= damage;
        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Health_Changed, (int)(health * 1000));
    }
}
