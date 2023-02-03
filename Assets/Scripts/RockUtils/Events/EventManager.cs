using System;
using System.Collections.Generic;
using UnityEngine;

namespace RockUtils {
    namespace GameEvents {
        public class EventManager {
            Dictionary<int, Action<int>> globalDictionary;
            Dictionary<Guid, Dictionary<int, Action<int>>> ownedDictionary;
            static EventManager eventManager;

            public static EventManager instance {
                get {
                    if (eventManager == null) {
                        eventManager = new EventManager();
                        eventManager.Init();
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
                        Dictionary<int, Action<int>> newDictionary = new Dictionary<int, Action<int>>();
                        newDictionary.Add(eventID, listener);
                        instance.ownedDictionary.Add(owner.Value, newDictionary);
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
                //  Owned Dictionary
                if (owner.HasValue) {
                    if (instance.ownedDictionary.TryGetValue(owner.Value, out Dictionary<int, Action<int>> thisDictionary)) {
                        if (thisDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                            thisEvent.Invoke(param);
                        }
                    }
                }
                //  Global Dictionary
                else {
                    if (instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                        thisEvent.Invoke(param);
                    }
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