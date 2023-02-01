using System;
using UnityEngine;
using RockUtils.GameEvents;

public abstract class Entity : MonoBehaviour {
    public enum EntityType {
        Undefined,
        Player,
        Mob,
        Interactable,
    }

    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    //  Note: [Rock]: We're hiding the inherited member of the same name using the new in front
    new Renderer renderer;
    Shader previousShader;
    Shader highlightShader;
    protected Guid entityID;

    public Locomotion locomotion { get; protected set; }
    public EntityStats stats { get; protected set; }
    public EntityAnimator animator { get; protected set; }
    public StatusEffectManager statusEffects { get; protected set; }
    public TargetingManager targetingManager { get; protected set; }

    public abstract EntityType GetEntityType();
    public abstract TargetingManager.TargetType GetTargetType();

    public Guid GetEntityID() { return entityID; }

    protected virtual Color? GetHighlightColor() { return null; }
    protected virtual Color? GetHighlightOutlineColor() { return null; }
    protected bool HasHighlightColor() { return GetHighlightColor().HasValue || GetHighlightOutlineColor().HasValue; }

    void Start() {
        Initialize();

        RegisterEvents();

        RegisterComponents();

        RegisterStats();
    }
    void OnDisable() {
        UnregisterEvents();

        UnregisterComponents();
    }

    protected virtual void Initialize() {
        entityID = Guid.NewGuid();
    }

    protected virtual void RegisterStats() { }

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected virtual void RegisterComponents() {
        stats = gameObject.AddComponent<EntityStats>();

        statusEffects = new StatusEffectManager();
        statusEffects.Setup(this);

        targetingManager = new TargetingManager();
        targetingManager.Setup(this);

        renderer = gameObject.GetComponentInChildren<Renderer>();
        highlightShader = Shader.Find("Custom/Entity_Outline");
    }

    protected virtual void UnregisterComponents() {
        targetingManager.Breakdown();
    }

    protected void AddEvent(Guid? owner, int eventID, Action<int> listener) {
        EventManager.StartListening(owner, eventID, listener);
    }

    protected void RemoveEvent(Guid? owner, int eventID, Action<int> listener) {
        EventManager.StopListening(owner, eventID, listener);
    }

    //  TODO: [Rock]: We need support for entities to be able to say 'nah' to status effects and the applying fails
    public virtual void OnStatusEffectApplied(StatusEffect effect) { }

    public virtual void OnStatusEffectRemoved(StatusEffect effect) { }

    //  Pre-Update - handle anything that needs to be done prior to the entity trying to act. For example, expiring status effects.
    protected virtual void PreUpdateStep() {
        //  Update this entities status effects. We handle this first so if their time expires we can clear them before the AI Step
        statusEffects.Update();
    }

    //  Update - This is where a brunt of the logic for entities will be handled from
    protected virtual void UpdateStep() { }

    //  Post-Update - This is where we can handle any last minute things before we're done for this tick with the entity
    protected virtual void PostUpdateStep() { }

    private void Update() {
        PreUpdateStep();
        UpdateStep();
        PostUpdateStep();
    }

    public virtual void OnSelected() {
        if (HasHighlightColor()) {
            previousShader = renderer.material.shader;
            renderer.material.shader = highlightShader;

            Color? highlight = GetHighlightColor();
            Color? outline = GetHighlightOutlineColor();

            if (highlight.HasValue) {
                renderer.material.SetColor("_FirstOutlineColor", highlight.Value);
            }
            if (outline.HasValue) {
                renderer.material.SetColor("_SecondOutlineColor", outline.Value);
            }
        }
    }

    public virtual void OnDeselected() {
        if (HasHighlightColor()) {
            renderer.material.shader = previousShader;
        }
    }
}
