using UnityEngine;

public abstract class LivingEntity : Entity {
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public Locomotion locomotion { get; protected set; }
    public EntityStats stats { get; protected set; }
    public StatusEffectManager statusEffects { get; protected set; }
    public TargetingManager targetingManager { get; protected set; }

    AttributeDictionary attributes;

    public override EntityType GetEntityType() { return EntityType.LivingEntity; }

    protected override void Setup() {
        base.Setup();

        RegisterEvents();

        RegisterComponents();

        RegisterAttributes();
    }

    protected override void Breakdown() {
        base.Breakdown();

        UnregisterEvents();

        UnregisterComponents();
    }

    protected virtual void RegisterComponents() {
        stats = gameObject.AddComponent<EntityStats>();

        statusEffects = new StatusEffectManager();
        statusEffects.Setup(this);

        targetingManager = new TargetingManager();
        targetingManager.Setup(this);
    }

    protected virtual void UnregisterComponents() {
        targetingManager.Breakdown();
    }

    protected virtual void RegisterAttributes() {
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MAX_HEALTH);
        GetAttributes().RegisterAttribute(LivingEntitySharedAttributes.MOVEMENT_SPEED);
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
            attributes = new AttributeDictionary();
        }

        return attributes;
    }

    public AttributeInstance GetAttribute(Attribute attribute) {
        return GetAttributes().GetInstance(attribute);
    }
}
