﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace RockUtils {
    namespace GameEvents {
        public class EventManager : Singleton<EventManager> {
            private readonly Dictionary<GameEvents, Action<int>> globalDictionary = new Dictionary<GameEvents, Action<int>>();
            private readonly Dictionary<Guid, Dictionary<GameEvents, Action<int>>> ownedDictionary = new Dictionary<Guid, Dictionary<GameEvents, Action<int>>>();

            public static void StartListening(Guid? owner, GameEvents eventID, Action<int> listener) {
                if (Instance == null) {
                    return;
                }

                //  Owned Dictionary
                if (owner.HasValue) {
                    if (Instance.ownedDictionary.TryGetValue(owner.Value, out Dictionary<GameEvents, Action<int>> thisDictionary)) {
                        if (thisDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                            thisEvent += listener;
                            Instance.ownedDictionary[owner.Value][eventID] = thisEvent;
                        } else {
                            thisEvent += listener;
                            thisDictionary.Add(eventID, thisEvent);
                            Instance.ownedDictionary[owner.Value] = thisDictionary;
                        }
                    } else {
                        Dictionary<GameEvents, Action<int>> newDictionary = new Dictionary<GameEvents, Action<int>>();
                        newDictionary.Add(eventID, listener);
                        Instance.ownedDictionary.Add(owner.Value, newDictionary);
                    }
                }
                //  Global Dictionary
                else {
                    if (Instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                        thisEvent += listener;
                        Instance.globalDictionary[eventID] = thisEvent;
                    } else {
                        thisEvent += listener;
                        Instance.globalDictionary.Add(eventID, thisEvent);
                    }
                }
                
                //  Note: [Rock]: For now, we'll ignore the Input Manager and just treat it as if anyone can use it even
                //                  though the only thing using it is the player.
                //  If it's a keyboard button, we want to then add a dictionary entry into the input
                int keyID = InputManager.GameEventToKeyCode((int) eventID);
                if (keyID != -1) {
                    InputManager.AddInputListener((KeyCode) keyID, listener);
                }
            }

            public static void StartListening(GameEvents eventID, Action<int> listener) {
                StartListening(null, eventID, listener);
            }

            public static void StopListening(Guid? owner, GameEvents eventID, Action<int> listener) {
                if (Instance == null) {
                    return;
                }

                //  Owned Dictionary
                if (owner.HasValue) {
                    if (Instance.ownedDictionary.TryGetValue(owner.Value, out Dictionary<GameEvents, Action<int>> thisDictionary)) {
                        if (thisDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                            thisEvent -= listener;
                            Instance.ownedDictionary[owner.Value][eventID] = thisEvent;
                        }
                    }
                }
                //  Global Dictionary
                else {
                    if (Instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                        thisEvent -= listener;
                        Instance.globalDictionary[eventID] = thisEvent;
                    }
                }

                //  Note: [Rock]: For now, we'll ignore the Input Manager and just treat it as if anyone can use it even
                //                  though the only thing using it is the player.
                //  If it's a keyboard button, we want to then remove a dictionary entry out of the input
                int keyID = InputManager.GameEventToKeyCode((int) eventID);
                if (keyID != -1) {
                    InputManager.RemoveInputListener((KeyCode) keyID, listener);
                }
            }

            public static void StopListening(GameEvents eventID, Action<int> listener) {
                StopListening(null, eventID, listener);
            }

            public static void TriggerEvent(Guid? owner, GameEvents eventID, int param) {
                if (Instance == null) {
                    return;
                }

                //  Owned Dictionary
                if (owner.HasValue) {
                    if (Instance.ownedDictionary.TryGetValue(owner.Value, out Dictionary<GameEvents, Action<int>> thisDictionary)) {
                        if (thisDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                            thisEvent.Invoke(param);
                        }
                    }
                }
                //  Global Dictionary
                else {
                    if (Instance.globalDictionary.TryGetValue(eventID, out Action<int> thisEvent)) {
                        thisEvent.Invoke(param);
                    }
                }
            }

            public static void TriggerEvent(Guid? owner, GameEvents eventID) {
                TriggerEvent(owner, eventID, 0);
            }

            public static void TriggerEvent(GameEvents eventID, int param) {
                TriggerEvent(null, eventID, param);
            }

            public static void TriggerEvent(GameEvents eventID) {
                TriggerEvent(null, eventID, 0);
            }
        }
    }
}