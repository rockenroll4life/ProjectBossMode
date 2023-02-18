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

    Level level;
    public Renderer rendererToOutline;
    Shader previousShader;
    Shader highlightShader;
    protected Guid entityID;

    public abstract EntityType GetEntityType();
    public abstract Type GetSystemType();
    public abstract bool IsDead();

    public Guid GetEntityID() { return entityID; }
    protected Level GetLevel() => level;

    protected virtual Color? GetHighlightColor() { return null; }
    protected virtual Color? GetHighlightOutlineColor() { return null; }
    protected bool HasHighlightColor() { return GetHighlightColor().HasValue || GetHighlightOutlineColor().HasValue; }

    public virtual void Setup(Level level) {
        this.level = level;
        entityID = Guid.NewGuid();

        highlightShader = Shader.Find("Custom/Entity_Outline");

        level.GetWorldEvents().onEntitySpawned?.Invoke(this);
    }
    public virtual void Breakdown() {
        level.GetWorldEvents().onEntityKilled?.Invoke(this);
    }

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected void AddGlobalEvent(int eventID, Action<int> listener) { AddEvent(null, eventID, listener); }
    protected void AddOwnedEvent(int eventID, Action<int> listener) { AddEvent(entityID, eventID, listener); }

    protected void RemoveGlobalEvent(int eventID, Action<int> listener) { RemoveEvent(null, eventID, listener); }
    protected void RemoveOwnedEvent(int eventID, Action<int> listener) { RemoveEvent(entityID, eventID, listener); }

    void AddEvent(Guid? owner, int eventID, Action<int> listener) { EventManager.StartListening(owner, eventID, listener); }
    void RemoveEvent(Guid? owner, int eventID, Action<int> listener) { EventManager.StopListening(owner, eventID, listener); }

    //  Pre-Update - handle anything that needs to be done prior to the entity trying to act. For example, expiring status effects.
    protected virtual void PreUpdateStep() { }

    //  Update - This is where a brunt of the logic for entities will be handled from
    protected virtual void UpdateStep() { }

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
