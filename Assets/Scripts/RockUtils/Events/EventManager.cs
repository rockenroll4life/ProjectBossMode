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

            Dictionary<int, Action<int>> globalDictionary;
            Dictionary<Guid, Dictionary<int, Action<int>>> ownedDictionary;
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
                if (globalDictionary == null) {
                    globalDictionary = new Dictionary<int, Action<int>>();
                }
                if (ownedDictionary == null) {
                    ownedDictionary = new Dictionary<Guid, Dictionary<int, Action<int>>>();
                }
            }

            public static void StartListening(Guid? owner, int eventID, Action<int> listener) {
                //  Owned Dictionary
                if (owner.HasValue) {
                    if (instance.ownedDictionary.TryGetValue(owner.Value, out Dictionary<int, Action<int>> thisDictionary)) {
                        if (thisDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                            thisEvent += listener;
                            instance.ownedDictionary[owner.Value][eventID] = thisEvent;
                        } else {
                            thisEvent += listener;
                            thisDictionary.Add(eventID, thisEvent);
                            instance.ownedDictionary[owner.Value] = thisDictionary;
                        }
                    } else {
                        thisDictionary.Add(eventID, listener);
                        instance.ownedDictionary.Add(owner.Value, thisDictionary);
                    }
                }
                //  Global Dictionary
                else {
                    if (instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                        thisEvent += listener;
                        instance.globalDictionary[eventID] = thisEvent;
                    } else {
                        thisEvent += listener;
                        instance.globalDictionary.Add(eventID, thisEvent);
                    }
                }
                
                //  Note: [Rock]: For now, we'll ignore the Input Manager and just treat it as if anyone can use it even
                //                  though the only thing using it is the player.
                //  If it's a keyboard button, we want to then add a dictionary entry into the input
                int keyID = InputManager.GameEventToKeyCode(eventID);
                if (keyID != -1) {
                    InputManager.AddInputListener((KeyCode) keyID, listener);
                }
            }

            public static void StartListening(int eventID, Action<int> listener) {
                StartListening(null, eventID, listener);
            }

            public static void StopListening(Guid? owner, int eventID, Action<int> listener) {
                //  NOTE: [Rock]: For some reason we were doing a null check here, however I don't think we need it...monitor this...
                /*if (eventManager == null) {
                    return;
                }*/

                //  Owned Dictionary
                if (owner.HasValue) {
                    if (instance.ownedDictionary.TryGetValue(owner.Value, out Dictionary<int, Action<int>> thisDictionary)) {
                        if (thisDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                            thisEvent -= listener;
                            instance.ownedDictionary[owner.Value][eventID] = thisEvent;
                        }
                    }
                }
                //  Global Dictionary
                else {
                    if (instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                        thisEvent -= listener;
                        instance.globalDictionary[eventID] = thisEvent;
                    }
                }

                //  Note: [Rock]: For now, we'll ignore the Input Manager and just treat it as if anyone can use it even
                //                  though the only thing using it is the player.
                //  If it's a keyboard button, we want to then remove a dictionary entry out of the input
                int keyID = InputManager.GameEventToKeyCode(eventID);
                if (keyID != -1) {
                    InputManager.RemoveInputListener((KeyCode) keyID, listener);
                }
            }

            public static void StopListening(int eventID, Action<int> listener) {
                StopListening(null, eventID, listener);
            }

            public static void TriggerEvent(Guid? owner, int eventID, int param) {
                if (instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                    thisEvent.Invoke(param);
                }
            }

            public static void TriggerEvent(Guid? owner, int eventID) {
                TriggerEvent(owner, eventID, 0);
            }

            public static void TriggerEvent(int eventID, int param) {
                TriggerEvent(null, eventID, param);
            }

            public static void TriggerEvent(int eventID) {
                TriggerEvent(null, eventID, 0);
            }
        }
    }
}