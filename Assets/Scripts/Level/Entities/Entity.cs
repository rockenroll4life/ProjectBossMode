using System;
using UnityEngine;
using RockUtils.GameEvents;

public abstract class Entity : MonoBehaviour {
    //  TODO: [Rock]: Implement the Entity Types as a bit flag so we can have things of multiple types
    public enum EntityType {
        Undefined,
        LivingEntity,
        Player,
        Mob,
        Interactable,
        Destructable,
    }

    public Renderer rendererToOutline;

    protected Guid entityID;

    private Level level;
    private Shader previousShader;
    private Shader highlightShader;
    private EntityData entityData;
    private AttributeDictionary attributes;

    public abstract EntityType GetEntityType();
    public abstract Type GetSystemType();
    public virtual bool IsDead() => GetEntityData(EntityDataType.Health) <= 0;

    public Guid GetEntityID() { return entityID; }
    public Level GetLevel() => level;
    public float GetEntityData(EntityDataType type) => entityData.Get(type);
    public void SetEntityData(EntityDataType type, float value) => entityData.Set(type, value);

    protected virtual Color? GetHighlightColor() => null;
    protected virtual Color? GetHighlightOutlineColor() => null;
    protected bool HasHighlightColor() => GetHighlightColor().HasValue || GetHighlightOutlineColor().HasValue;

    public virtual void Setup(Level level) {
        this.level = level;
        entityID = Guid.NewGuid();

        highlightShader = Shader.Find("Custom/Entity_Outline");

        RegisterAttributes();
        RegisterComponents();

        level.GetWorldEvents().onEntitySpawned?.Invoke(this);
    }
    public virtual void Breakdown() {
        UnregisterComponents();

        level.GetWorldEvents().onEntityKilled?.Invoke(this);
    }

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

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

        GetAttributes().RegisterAttribute(AttributeTypes.CooldownReduction);
        GetAttributes().RegisterAttribute(AttributeTypes.ResourceCostReduction);

        entityData = new EntityData(this);
    }

    protected virtual void RegisterComponents() { }
    protected virtual void UnregisterComponents() { }

    protected void AddGlobalEvent(GameEvents eventID, Action<int> listener) { AddEvent(null, eventID, listener); }
    protected void AddOwnedEvent(GameEvents eventID, Action<int> listener) { AddEvent(entityID, eventID, listener); }

    protected void RemoveGlobalEvent(GameEvents eventID, Action<int> listener) { RemoveEvent(null, eventID, listener); }
    protected void RemoveOwnedEvent(GameEvents eventID, Action<int> listener) { RemoveEvent(entityID, eventID, listener); }

    void AddEvent(Guid? owner, GameEvents eventID, Action<int> listener) { EventManager.StartListening(owner, eventID, listener); }
    void RemoveEvent(Guid? owner, GameEvents eventID, Action<int> listener) { EventManager.StopListening(owner, eventID, listener); }

    public AttributeDictionary GetAttributes() {
        if (attributes == null) {
            attributes = new AttributeDictionary(this);
        }

        return attributes;
    }

    public IAttributeInstance GetAttribute(AttributeTypes attribute) {
        return GetAttributes().GetInstance(Attributes.Get(attribute));
    }

    //  Pre-Update - handle anything that needs to be done prior to the entity trying to act. For example, expiring status effects.
    protected virtual void PreUpdateStep() { }

    //  Update - This is where a brunt of the logic for entities will be handled from
    protected virtual void UpdateStep() { entityData.Update(); }

    //  Post-Update - This is where we can handle any last minute things before we're done for this tick with the entity
    protected virtual void PostUpdateStep() { }

    public void Tick() {
        PreUpdateStep();
        UpdateStep();
        PostUpdateStep();
    }

    public virtual void OnSelected() {
        if (HasHighlightColor()) {
            previousShader = rendererToOutline.material.shader;
            rendererToOutline.material.shader = highlightShader;

            Color? highlight = GetHighlightColor();
            Color? outline = GetHighlightOutlineColor();

            if (highlight.HasValue) {
                rendererToOutline.material.SetColor("_FirstOutlineColor", highlight.Value);
            }
            if (outline.HasValue) {
                rendererToOutline.material.SetColor("_SecondOutlineColor", outline.Value);
            }
        }
    }

    public virtual void OnDeselected() {
        if (rendererToOutline && HasHighlightColor()) {
            rendererToOutline.material.shader = previousShader;
        }
    }
}
