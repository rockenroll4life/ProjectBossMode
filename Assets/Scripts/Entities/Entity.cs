﻿using System;
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

    //  Hidden values, we don't really need to go setting lots of things when we can hide it behind the scenes
    Color _highlight;
    Color _highlightOutline;
    protected Color highlightColor {
        get {
            return _highlight;
        }

        set {
            _highlight = value;
            _highlightOutline = new Color(value.r, value.g, value.b, 0.5f);
        }
    }

    public Locomotion locomotion { get; protected set; }
    public EntityStats stats { get; protected set; }
    public EntityAnimator animator { get; protected set; }

    public StatusEffectManager statusEffects { get; protected set; }

    public EntityType entityType { get; protected set; }

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

    public void OnStartHovering() {
        previousShader = renderer.material.shader;
        renderer.material.shader = highlightShader;

        renderer.material.SetColor("_FirstOutlineColor", _highlight);
        renderer.material.SetColor("_SecondOutlineColor", _highlightOutline);
    }

    public void OnStopHovering() {
        renderer.material.shader = previousShader;
    }
}