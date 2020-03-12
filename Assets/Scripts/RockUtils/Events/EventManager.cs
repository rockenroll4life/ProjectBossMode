using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    Dictionary<int, Action> dictionary;
    static EventManager eventManager;

    public static EventManager instance {
        get {
            if (!eventManager) {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager) {
                    Debug.LogError("Using this requires an EventManager on a GameObject within the scene");
                } else {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init() {
        if (dictionary == null) {
            dictionary = new Dictionary<int, Action>();
        }
    }

    public static void StartListening(int eventID, Action listener) {
        if (instance.dictionary.TryGetValue(eventID, out Action thisEvent)) {
            thisEvent += listener;
            instance.dictionary[eventID] = thisEvent;
        } else {
            thisEvent += listener;
            instance.dictionary.Add(eventID, thisEvent);
        }
    }

    public static void StopListening(int eventID, Action listener) {
        if (eventManager == null) {
            return;
        }

        if (instance.dictionary.TryGetValue(eventID, out Action thisEvent)) {
            thisEvent -= listener;
            instance.dictionary[eventID] = thisEvent;
        }
    }

    public static void TriggerEvent(int eventID) {
        if (instance.dictionary.TryGetValue(eventID, out Action thisEvent)) {
            thisEvent.Invoke();
        }
    }
}