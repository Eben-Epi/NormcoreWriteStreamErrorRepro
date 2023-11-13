using System;
using Normal;
using Normal.Realtime;
using Normal.Utility;
using UnityEngine;


    public class B2BRealtime : Realtime
    {
        private static B2BRealtime _instance;

        private void Update()
        {
            if (_instance == null)
                _instance = this;
            if (room == null)
                return;
            room.debugLogging = NormcoreProjectSettings.logLevel == NormcoreLogLevel.Debug;
            room.Tick((double) Time.unscaledDeltaTime);
        }

        public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation,
            InstantiateOptions options)
        {
            if (_instance != null)
                options.useInstance = _instance;
            if (OfflineManager.isOffline)
                return GameObject.Instantiate(Resources.Load(prefabName), position, rotation) as GameObject;
            else
                return Realtime.Instantiate(prefabName, position, rotation, options);
        }

        public new static GameObject Instantiate(string prefabName, InstantiateOptions? options)
        {
            if (options != null && _instance != null)
            {
                var newOptions = options.Value;

                newOptions.useInstance = _instance;
                options = newOptions;
            }
            if (OfflineManager.isOffline)
                return GameObject.Instantiate(Resources.Load(prefabName)) as GameObject;
            else
                return Realtime.Instantiate(prefabName, options);
        }

        public new static void Destroy(GameObject g)
        {
            if (OfflineManager.isOffline)
            {
                GameObject.Destroy(g);
            }
            else
            {
                Realtime.Destroy(g);
            }
        }
    }