using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SimpleBus.Internal
{
    public static class EventBusUtil
    {
        public static IReadOnlyList<Type> EventTypes { get; set; }
        public static IReadOnlyList<Type> EventBusTypes { get; set; }

        #if UNITY_EDITOR
        public static PlayModeStateChange PlayModeState { get; set; }

        [InitializeOnLoadMethod]
        public static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeState = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ClearAllBuses();
            }
        }
        #endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBuses();
        }
        
        public static void ClearAllBuses()
        {
            Debug.Log($"Clearing all buses...");
            for (int i = 0; i < EventBusTypes.Count; i++)
            {
                var bustype = EventBusTypes[i];
                var clearMethodInfo = bustype.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethodInfo.Invoke(null, null);
            }
        }

        static IReadOnlyList<Type> InitializeAllBuses()
        {
            List<Type> eventBusTypes = new List<Type>();

            var typedef = typeof(EventBus<>);

            foreach (var eventBusType in EventTypes)
            {
                var busType = typedef.MakeGenericType(eventBusType);
                eventBusTypes.Add(busType);
                Debug.Log($"Initialized {eventBusType.Name}");
            }
            
            return eventBusTypes;
        }
        
       
    }
}