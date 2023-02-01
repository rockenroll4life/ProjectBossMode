using System;
using System.Collections.Generic;
using UnityEngine;

namespace RockUtils {
    namespace GameEvents {
        public class EventManager : MonoBehaviour
        {
            //  TODO: [Rock]: Add increase support to the event manager
            //  Events should be broken down into two types of events
            //      1. Global Events - These events are just generic type of events that anyone might want to listen to, for example, the Pause GameEvent.
            //      2. Targeted Events - These events are specifically targeted towards a specific entity. for example, something might update the players health and the player
            //                              and UI would want to listen to a specific event targeting the player for the Health Changing.

            //  NOTE: [ROCK]: This could probably be implemented easily by adding a nullable GUID value version of the functions. After that
            //                  store two versions of the dictionary, the current one as a "Global" and a separate one that's a Dictionary<GUID, Dictionary<int, Action<int>>>
            //                  That contains all the events for a given entity. If we pass a GUID to the trigger we trigger those, else we make the call on the Global

            Dictionary<int, Action<int>> dictionary;
            static EventManager eventManager;

            public static EventManager instance {
                get {
                    if (!eventManager) {
                        eventManager = FindObjectOfType<EventManager>();

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
                if (instance.dictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                    thisEvent += listener;
                    instance.dictionary[eventID] = thisEvent;
                } else {
                    thisEvent += listener;
                    instance.dictionary.Add(eventID, thisEvent);
                }

                //  If it's a keyboard button, we want to then add a dictionary entry into the input
                int keyID = InputManager.GameEventToKeyCode(eventID);
                if (keyID != -1) {
                    InputManager.AddInputListener((KeyCode) keyID, listener);
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

                //  If it's a keyboard button, we want to then remove a dictionary entry out of the input
                int keyID = InputManager.GameEventToKeyCode(eventID);
                if (keyID != -1) {
                    InputManager.RemoveInputListener((KeyCode) keyID, listener);
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
    }
}