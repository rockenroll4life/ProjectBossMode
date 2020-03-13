using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public Locomotion locomotion { get; protected set; }
    public EntityStats stats { get; protected set; }
    public EntityAnimator animator { get; protected set; }

    protected virtual void Start() {
        RegisterEvents();

        animator = gameObject.AddComponent<EntityAnimator>();
        animator.SetOwner(this);

        stats = gameObject.AddComponent<EntityStats>();
        RegisterStats();
    }
    protected virtual void OnDisable() {
        UnregisterEvents();
    }

    protected virtual void RegisterStats() { }

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected void AddEvent(int eventID, Action<int> listener) {
        EventManager.StartListening(eventID, listener);
    }

    protected void RemoveEvent(int eventID, Action<int> listener) {
        EventManager.StopListening(eventID, listener);
    }

    protected virtual void Update() { }
}
