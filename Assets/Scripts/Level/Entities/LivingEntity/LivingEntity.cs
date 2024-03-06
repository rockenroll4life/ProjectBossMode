using UnityEngine;
using RockUtils.GameEvents;

public abstract class LivingEntity : Entity, IDamageable {
    public static readonly AttributeTypes[] ABILITY_COOLDOWNS =
        { AttributeTypes.Ability1Cooldown, AttributeTypes.Ability2Cooldown, AttributeTypes.Ability3Cooldown, AttributeTypes.Ability4Cooldown, AttributeTypes.UltimateCooldown };

    public GameObject attackProjectilePrefab;

    protected StatusEffectManager statusEffects;
    protected AbilityManager abilities;
    protected Locomotion locomotion;
    protected EntityAnimator animator;
    protected ITargeter targeter;
    AttributeDictionary attributes;

    float attackTimer = 0;

    EntityData entityData;

    //  NOTE: [Rock]: This is LivingEntity for now...but not sure if we need to change this to Entity instead...
    LivingEntity lastDamager = null;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.LivingEntity;
    public override System.Type GetSystemType() => typeof(LivingEntity);
    public override bool IsDead() => GetEntityData(EntityDataType.Health) <= 0;
    public LivingEntity GetLastDamager() => lastDamager;

    public ITargeter GetTargeter() => targeter;
    public Locomotion GetLocomotion() => locomotion;
    public AbilityManager GetAbilities() => abilities;

    public float GetEntityData(EntityDataType type) => entityData.Get(type);
    public void SetEntityData(EntityDataType type, float value) => entityData.Set(type, value);

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
        GetAttributes().RegisterAttribute(AttributeTypes.HealthMax);
        GetAttributes().RegisterAttribute(AttributeTypes.HealthRegenRate);

        GetAttributes().RegisterAttribute(AttributeTypes.ManaMax);
        GetAttributes().RegisterAttribute(AttributeTypes.ManaRegenRate);

        GetAttributes().RegisterAttribute(AttributeTypes.MovementSpeed);

        GetAttributes().RegisterAttribute(AttributeTypes.AttackDamage);
        GetAttributes().RegisterAttribute(AttributeTypes.AttackSpeed);
        GetAttributes().RegisterAttribute(AttributeTypes.AttackRange);

        GetAttributes().RegisterAttribute(AttributeTypes.Ability1Cooldown);
        GetAttributes().RegisterAttribute(AttributeTypes.Ability2Cooldown);
        GetAttributes().RegisterAttribute(AttributeTypes.Ability3Cooldown);
        GetAttributes().RegisterAttribute(AttributeTypes.Ability4Cooldown);
        GetAttributes().RegisterAttribute(AttributeTypes.UltimateCooldown);

        entityData = new EntityData(entityID);

        SetEntityData(EntityDataType.Health, GetAttribute(AttributeTypes.HealthMax).GetValue());
    }

    protected virtual void RegisterAbilities() { }

    public virtual bool CanApplyStatusEffect(StatusEffect effect) => true;

    public void AddStatusEffect(StatusEffect effect) {
        if (CanApplyStatusEffect(effect)) {
            statusEffects.AddStatusEffect(effect);
        }
    }

    public void RemoveStatusEffect(StatusEffect effect) {
        statusEffects.RemoveStatusEffect(effect);
    }

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
        float health = GetEntityData(EntityDataType.Health);
        float oldHealth = health;
        health += GetAttribute(AttributeTypes.HealthRegenRate).GetValue() * Time.deltaTime;
        health = Mathf.Clamp(health, 0, GetAttribute(AttributeTypes.HealthMax).GetValue());
        SetEntityData(EntityDataType.Health, health);
        if (health != oldHealth) {
            EventManager.TriggerEvent(GetEntityID(), GameEvents.Entity_Data_Changed + (int)EntityDataType.Health, (int) (health * 1000));
        }

        //  Update Mana
        float mana = GetEntityData(EntityDataType.Mana);
        float oldMana = mana;
        mana += GetAttribute(AttributeTypes.HealthRegenRate).GetValue() * Time.deltaTime;
        mana = Mathf.Clamp(mana, 0, GetAttribute(AttributeTypes.ManaMax).GetValue());
        SetEntityData(EntityDataType.Mana, mana);
        if (mana != oldMana) {
            EventManager.TriggerEvent(GetEntityID(), GameEvents.Entity_Data_Changed + (int) EntityDataType.Mana, (int) (mana * 1000));
        }
    }

    public AttributeDictionary GetAttributes() {
        if (attributes == null) {
            attributes = new AttributeDictionary(this);
        }

        return attributes;
    }

    public IAttributeInstance GetAttribute(AttributeTypes attribute) {
        return GetAttributes().GetInstance(Attributes.Get(attribute));
    }

    protected virtual bool CanAttack() {
        if (attackTimer <= 0) {
            IDamageable target = targeter.GetTargetedEntity();
            if (target != null && target.GetEntity() != null) {
                float attackRange = GetAttribute(AttributeTypes.AttackRange).GetValue();
                return (target.GetEntity().transform.position - transform.position).sqrMagnitude <= (attackRange * attackRange);
            }
        }

        return false;
    }

    protected virtual void Attack() {
        attackTimer = GetAttribute(AttributeTypes.AttackSpeed).GetValue();

        Vector3 offset = (transform.forward * 1) + Vector3.up;
        Projectile proj = Instantiate(attackProjectilePrefab, transform.position + offset, transform.rotation).GetComponent<Projectile>();
        proj.Setup(this, targeter.GetTargetedEntity(), GetAttribute(AttributeTypes.AttackDamage).GetValue());
    }

    public void DealDamage(Entity damager, float damage) {
        SetEntityData(EntityDataType.Health, GetEntityData(EntityDataType.Health) - damage);

        if (damager is LivingEntity livingEntity) {
            lastDamager = livingEntity;
        }

        EventManager.TriggerEvent(GetEntityID(), GameEvents.LivingEntity_Hurt, (int) (damage * 1000));
        EventManager.TriggerEvent(GetEntityID(), GameEvents.Entity_Data_Changed + (int) EntityDataType.Health, (int) (GetEntityData(EntityDataType.Health) * 1000));
    }
}
