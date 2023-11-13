using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    public class OfflineManager : MonoBehaviour
    {
        private bool _isOffline;
        public static bool isOffline
        {
            get => instance._isOffline;
            set
            {
                instance._offlineModels.Clear();
                instance._isOffline = value;
            }
        }
        public static OfflineManager instance;
        private readonly List<IOfflineModel> _offlineModels = new();

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void StartOfflineMode()
        {
            isOffline = true;
            SceneManager.LoadSceneAsync("GameBase2");
        }

        public static void AddOfflineModel(IOfflineModel m)
        {
             instance._offlineModels.Add(m);
        }

        private void ManageOfflineModels()
        {
            foreach (var offlineModel in _offlineModels)
            {
                offlineModel?.ManageOfflineCallbacks();
            }
        }

        private void Update()
        {
            if (isOffline)
                ManageOfflineModels();
        }
    }