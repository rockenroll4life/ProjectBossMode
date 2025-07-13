using UnityEngine;
using RockUtils.GameEvents;

public abstract class LivingEntity : Entity, IDamageable {
    public GameObject attackProjectilePrefab;

    protected StatusEffectManager statusEffects;
    protected Abilities abilities;
    protected SpellIndicators spellIndicators;
    protected Locomotion locomotion;
    protected EntityAnimator animator;
    protected ITargeter targeter;
    
    private float attackTimer = 0;

    public Entity GetEntity() => this;
    public override EntityType GetEntityType() => EntityType.LivingEntity;
    public override System.Type GetSystemType() => typeof(LivingEntity);

    //  NOTE: [Rock]: We can probably change these getters to properties
    public ITargeter GetTargeter() => targeter;
    public Locomotion GetLocomotion() => locomotion;
    public Abilities GetAbilities() => abilities;
    public SpellIndicators GetSpellIndicators() => spellIndicators;

    protected override void RegisterComponents() {
        base.RegisterComponents();

        statusEffects = new StatusEffectManager(this);

        abilities = new Abilities(this);
        RegisterAbilities();
    }

    protected override void RegisterAttributes() {
        base.RegisterAttributes();

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

        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        } else {
            if (CanAttack()) {
                Attack();
            }
        }
    }

    private void Update() {
        abilities.Update();
    }

    //  TODO: [Rock]: Remove this scaler and have the ResourceCost know it should scale it's value
    public void UseResource(ResourceCost cost, float scaler = 1f) {
        EntityDataType resourceType = cost.GetResourceType();

        float value = Mathf.Max(GetEntityData(resourceType) - (cost.GetCost() * scaler), 0);
        SetEntityData(resourceType, value);

        EventManager.TriggerEvent(GetEntityID(), GameEvents.Entity_Data_Changed + (int) resourceType, (int) (value * 1000));
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

    public void Hurt(Entity damager, float damage) {
        SetEntityData(EntityDataType.Health, GetEntityData(EntityDataType.Health) - damage);

        lastDamager = damager;

        EventManager.TriggerEvent(GetEntityID(), GameEvents.LivingEntity_Hurt, (int) (damage * 1000));
        EventManager.TriggerEvent(GetEntityID(), GameEvents.Entity_Data_Changed + (int) EntityDataType.Health, (int) (GetEntityData(EntityDataType.Health) * 1000));
    }
}
