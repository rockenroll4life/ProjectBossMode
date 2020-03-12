using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public GameObject handAttachRight;
    public GameObject handAttachLeft;

    public Locomotion locomotion { get; protected set; }
    protected EntityAnimator animator;

    protected virtual void Start() {
        RegisterEvents();

        animator = gameObject.AddComponent<EntityAnimator>();
        animator.SetOwner(this);
    }
    protected virtual void OnDisable() {
        UnregisterEvents();
    }

    protected virtual void RegisterEvents() { }
    protected virtual void UnregisterEvents() { }

    protected void AddEvent(int eventID, Action listener) {
        EventManager.StartListening(eventID, listener);
    }

    protected void RemoveEvent(int eventID, Action listener) {
        EventManager.StopListening(eventID, listener);
    }

    protected virtual void Update() { }
}
