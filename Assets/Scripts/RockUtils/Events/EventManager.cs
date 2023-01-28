using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
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