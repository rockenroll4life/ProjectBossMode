using UnityEngine;

public abstract class LivingEntity : Entity {
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public Locomotion locomotion { get; protected set; }
    public StatusEffectManager statusEffects { get; protected set; }
    
    protected Targeter targeter;
    AttributeDictionary attributes;

    public override EntityType GetEntityType() { return EntityType.LivingEntity; }

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
    }

    //  TODO: [Rock]: We need support for entities to be able to say 'nah' to status effects and the applying fails
    public virtual void OnStatusEffectApplied(StatusEffect effect) { }

    public virtual void OnStatusEffectRemoved(StatusEffect effect) { }

    protected override void PreUpdateStep() {
        base.PreUpdateStep();

        //  Update this entities status effects. We handle this first so if their time expires we can clear them before the AI Step
        statusEffects.Update();
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
}
