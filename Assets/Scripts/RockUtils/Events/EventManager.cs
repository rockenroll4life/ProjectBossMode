using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    //  TODO: [Rock]: Add increase support to the event manager
    //  Events should be broken down into two types of events
    //      1. Global Events - These events are just generic type of events that anyone might want to listen to, for example, the Pause GameEvent.
    //      2. Targeted Events - These events are specifically targeted towards a specific entity. for example, something might update the players health and the player
    //                              and UI would want to listen to a specific event targeting the player for the Health Changing.
    Dictionary<int, Action<int>> dictionary;
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
            dictionary = new Dictionary<int, Action<int>>();
        }
    }

    public static void StartListening(int eventID, Action<int> listener) {
        //  TODO: [Rock]: If the event ID is related to buttons, we should by default add the button to the input listeners so they properly trigger
        //  the game events instead of us having to go the backwards route and add a Input Event (That secretly adds a Game Event)

        if (instance.dictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
            thisEvent += listener;
            instance.dictionary[eventID] = thisEvent;
        } else {
            thisEvent += listener;
            instance.dictionary.Add(eventID, thisEvent);
        }
    }

    public static void StopListening(int eventID, Action<int> listener) {
        if (eventManager == null) {
            return;
        }

        if (instance.dictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
            thisEvent -= listener;
            instance.dictionary[eventID] = thisEvent;
        }
    }

    public static void TriggerEvent(int eventID, int param) {
        if (instance.dictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
            thisEvent.Invoke(param);
        }
    }

    public static void TriggerEvent(int eventID) {
        TriggerEvent(eventID, 0);
    }
}