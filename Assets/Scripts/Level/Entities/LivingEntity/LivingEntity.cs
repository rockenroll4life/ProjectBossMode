using UnityEngine;
using RockUtils.GameEvents;

public abstract class LivingEntity : Entity, IDamageable {
    public static readonly RangedAttribute ABILITY1_COOLDOWN = new("generic.ability1", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY2_COOLDOWN = new("generic.ability2", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY3_COOLDOWN = new("generic.ability3", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ABILITY4_COOLDOWN = new("generic.ability4", 5, 0, float.MaxValue);
    public static readonly RangedAttribute ULTIMATE_COOLDOWN = new("generic.ultimate", 5, 0, float.MaxValue);

    public static readonly RangedAttribute[] ABILITY_COOLDOWNS = { ABILITY1_COOLDOWN, ABILITY2_COOLDOWN, ABILITY3_COOLDOWN, ABILITY4_COOLDOWN, ULTIMATE_COOLDOWN };

    public GameObject attackProjectilePrefab;

    protected StatusEffectManager statusEffects;
    protected AbilityManager abilities;
    protected Locomotion locomotion;
    protected EntityAnimator animator;
    protected ITargeter targeter;
    AttributeDictionary attributes;

    float attackTimer = 0;

    //  TODO: [Rock]: Replace with EntityData
    float[] resources = new float[(int) ResourceType._COUNT];

    //  NOTE: [Rock]: This is LivingEntity for now...but not sure if we need to change this to Entity instead...
    LivingEntity lastDamager = null;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.LivingEntity;
    public override System.Type GetSystemType() => typeof(LivingEntity);
    public override bool IsDead() => GetResource(ResourceType.Health) <= 0;
    public LivingEntity GetLastDamager() => lastDamager;

    public ITargeter GetTargeter() => targeter;
    public Locomotion GetLocomotion() => locomotion;
    public AbilityManager GetAbilities() => abilities;

    public float GetResource(ResourceType type) => resources[(int) type];
    public void SetResource(ResourceType type, float value) => resources[(int) type] = value;

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

        abilities = new AbilityManager(this);
        RegisterAbilities();
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

        GetAttributes().RegisterAttribute(ABILITY1_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY2_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY3_COOLDOWN);
        GetAttributes().RegisterAttribute(ABILITY4_COOLDOWN);
        GetAttributes().RegisterAttribute(ULTIMATE_COOLDOWN);

        SetResource(ResourceType.Health, GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue());
    }

    protected virtual void RegisterAbilities() { }

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

        UpdateResources();
        abilities.Update();

        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        } else {
            if (CanAttack()) {
                Attack();
            }
        }
    }

    void UpdateResources() {
        //  Update Health
        float health = GetResource(ResourceType.Health);
        float oldHealth = health;
        health += GetAttribute(LivingEntitySharedAttributes.HEALTH_REGEN_RATE).GetValue() * Time.deltaTime;
        health = Mathf.Clamp(health, 0, GetAttribute(LivingEntitySharedAttributes.HEALTH_MAX).GetValue());
        SetResource(ResourceType.Health, health);
        if (health != oldHealth) {
            EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Health_Changed, (int) (health * 1000));
        }

        //  Update Mana
        float mana = GetResource(ResourceType.Mana);
        float oldMana = mana;
        mana += GetAttribute(LivingEntitySharedAttributes.MANA_REGEN_RATE).GetValue() * Time.deltaTime;
        mana = Mathf.Clamp(mana, 0, GetAttribute(LivingEntitySharedAttributes.MANA_MAX).GetValue());
        SetResource(ResourceType.Mana, mana);
        if (mana != oldMana) {
            EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Mana_Changed, (int) (mana * 1000));
        }
    }

    public AttributeDictionary GetAttributes() {
        if (attributes == null) {
            attributes = new AttributeDictionary(this);
        }

        return attributes;
    }

    public IAttributeInstance GetAttribute(IAttribute attribute) {
        return GetAttributes().GetInstance(attribute);
    }

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
        SetResource(ResourceType.Health, GetResource(ResourceType.Health) - damage);

        if (damager is LivingEntity livingEntity) {
            lastDamager = livingEntity;
        }

        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.LivingEntity_Hurt, (int) (damage * 1000));
        EventManager.TriggerEvent(GetEntityID(), (int) GameEvents.Health_Changed, (int) (GetResource(ResourceType.Health) * 1000));
    }
}
