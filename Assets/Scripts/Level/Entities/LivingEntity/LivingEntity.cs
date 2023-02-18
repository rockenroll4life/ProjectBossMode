using UnityEngine;
using RockUtils.GameEvents;

public abstract class LivingEntity : Entity, IDamageable {
    public GameObject attackProjectilePrefab;

    protected StatusEffectManager statusEffects;
    protected Locomotion locomotion;
    protected EntityAnimator animator;
    protected Targeter targeter;
    AttributeDictionary attributes;

    float attackTimer = 0;

    //  TODO: [Rock]: Replace with EntityData
    protected float health;

    //  NOTE: [Rock]: This is LivingEntity for now...but not sure if we need to change this to Entity instead...
    LivingEntity lastDamager = null;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.LivingEntity;
    public override System.Type GetSystemType() => typeof(LivingEntity);
    public override bool IsDead() => health <= 0;
    public LivingEntity GetLastDamager() => lastDamager;

    public Targeter GetTargeter() => targeter;
    public Locomotion GetLocomotion() => locomotion;

    public override void Setup(Level level) {
        base.Setup(level);

        RegisterEvents();
        RegisterAttributes();
        RegisterComponents();
    }

    public override void Breakdown() {
        base.Breakdown();

        UnregisterEvents();
        UnregisterComponents();
    }

    protected virtual void RegisterComponents() {
        statusEffects = new StatusEffectManager(this);
        locomotion = new Locomotion(this);
    }

    protected virtual void UnregisterComponents() { }

    protected virtual void RegisterAttributes() {
        //  These are the base attributes that every entity has, only register attributes here that everyone will have (Even if we set them to a value
        //  of 0 in the actual entity themselves.
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.HEALTH_MAX);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.HEALTH_REGEN_RATE);

        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MANA_MAX);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MANA_REGEN_RATE);

        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED);

        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.ATTACK_DAMAGE);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.ATTACK_SPEED);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.ATTACK_RANGE);

        health = GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue();
    }

    //  TODO: [Rock]: We need support for entities to be able to say 'nah' to status effects and the applying fails
    public virtual void OnStatusEffectApplied(StatusEffect effect) { }

    public virtual void OnStatusEffectRemoved(StatusEffect effect) { }

    protected override void PreUpdateStep() {
        base.PreUpdateStep();

        statusEffects.Update();
        locomotion.Update();
        animator.Update();
        targeter.Update();
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
        if (attackTimer <= 0) {
            IDamageable target = targeter.GetTargetedEntity();
            if (target != null && target.GetEntity() != null) {
                float attackRange = GetAttribute(LivingEntitySharedAttributes.ATTACK_RANGE).GetValue();
                return (target.GetEntity().transform.position - transform.position).sqrMagnitude <= (attackRange * attackRange);
            }
        }

        return false;
    }

    protected virtual void Attack() {
        attackTimer = GetAttribute(LivingEntitySharedAttributes.ATTACK_SPEED).GetValue();

        Vector3 offset = (transform.forward * 1) + Vector3.up;
        Projectile proj = Instantiate(attackProjectilePrefab, transform.position + offset, transform.rotation).GetComponent<Projectile>();
        proj.Setup(this, targeter.GetTargetedEntity(), GetAttribute(LivingEntitySharedAttributes.ATTACK_DAMAGE).GetValue());
    }

    public void DealDamage(Entity damager, float damage) {
        health -= damage;

        if (damager is LivingEntity livingEntity) {
            lastDamager = livingEntity;
        }

        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.LivingEntity_Hurt, (int) (damage * 1000));
        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Health_Changed, (int) (health * 1000));
    }
}
