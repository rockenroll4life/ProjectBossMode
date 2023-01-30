using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public enum EntityType {
        Undefined,
        Player,
        Mob,
        Interactable,
    }

    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public int entityID { private set; get; }
    Renderer renderer;
    Shader previousShader;
    Shader highlightShader;

    public Locomotion locomotion { get; protected set; }
    public EntityStats stats { get; protected set; }
    public EntityAnimator animator { get; protected set; }

    public StatusEffectManager statusEffects { get; protected set; }

    public abstract EntityType GetEntityType();
    public abstract TargetingManager.TargetType GetTargetType();

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
    }

    protected virtual void Initialize() { }

    protected virtual void RegisterStats() { }

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected virtual void RegisterComponents() {
        stats = gameObject.AddComponent<EntityStats>();

        statusEffects = new StatusEffectManager();
        statusEffects.Setup(this);

        renderer = gameObject.GetComponentInChildren<Renderer>();
        highlightShader = Shader.Find("Custom/Entity_Outline");
    }

    protected virtual void UnregisterComponents() {
        //  TODO: [Rock]: Make sure to Breakdown anything we need to
    }

    protected void AddEvent(int eventID, Action<int> listener) {
        EventManager.StartListening(eventID, listener);
    }

    protected void RemoveEvent(int eventID, Action<int> listener) {
        EventManager.StopListening(eventID, listener);
    }

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
