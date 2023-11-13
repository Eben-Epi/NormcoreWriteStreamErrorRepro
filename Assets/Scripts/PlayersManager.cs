using Normal.Realtime;
using UnityEngine;

public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemySpawner;
        public static bool startTuto = false;
        private Realtime _realtime;
        private B2BPlayer _cachedMate;
        private static PlayersManager _instance;
        private B2BPlayer _current;
        private static B2BPlayer _offlineCurrent;
        public static B2BPlayer shooter => current.role == PlayerRole.Shooting ? current : mate;
        public static B2BPlayer driver => current.role == PlayerRole.Driving ? current : mate;
        public static B2BPlayer current
        {
            get
            {
                if (!OfflineManager.isOffline)
                    return _instance != null ? _instance._current : null;
                if (_instance != null && _offlineCurrent == null)
                {
                    _offlineCurrent = B2BRealtime.Instantiate("B2BPlayer", new Realtime.InstantiateOptions
                    {
                        ownedByClient = true,
                        preventOwnershipTakeover = true,
                        destroyWhenOwnerLeaves = true,
                        destroyWhenLastClientLeaves = true
                    }).GetComponent<B2BPlayer>();
                }

                return _offlineCurrent;
            }
        }
        public static B2BPlayer mate
        {
            get
            {
                if (_instance == null || OfflineManager.isOffline) return null;
                if (_instance._cachedMate != null && !_instance._cachedMate.isDoomed)
                    return _instance._cachedMate;

                var ps = FindObjectsOfType<B2BPlayer>();

                foreach (var p in ps)
                {
                    if (!p.isOwnedLocallyInHierarchy && !p.isDoomed && current != null &&
                        p.GetInstanceID() != current.GetInstanceID())
                    {
                        _instance._cachedMate = p;
                        return p;
                    }
                }

                return null;
            }
        }

        private void Start()
        {
            Application.runInBackground = true;
            if (_instance != null)
            {
                // Debug.Log("Destroying PlayersManager.instance cause there already is one");
                StopAllCoroutines();
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            _realtime = GetComponent<Realtime>();
            _instance = this;
            if (startTuto)
            {
                startTuto = false;
            }
            else
            {
                _realtime.didConnectToRoom += DidConnectToRoom;
            }
        }
        

        public static void Destroy()
        {
            if (_instance == null) return;
            B2BRealtime.Destroy(current.gameObject);
            if (_instance._realtime.connected)
                _instance._realtime.Disconnect();
            Destroy(_instance.gameObject);
            _instance = null;
        }
        

        private void DidConnectToRoom(Realtime realtime)
        {
            var opt = new Realtime.InstantiateOptions
            {
                useInstance = realtime,
                ownedByClient = true,
                preventOwnershipTakeover = true,
                destroyWhenOwnerLeaves = true,
                destroyWhenLastClientLeaves = true
            };
            var B2Bplayer = B2BRealtime.Instantiate("B2BPlayer", opt);
            GameObject playerGameObject;
            if (Player.instance == null)
            {
                playerGameObject = B2BRealtime.Instantiate("Player", opt);
                if (!playerGameObject.TryGetComponent<Player>(out var player)) return;
                player.RequestPlayerOwnership();
            }

            _current = B2Bplayer.GetComponent<B2BPlayer>();
            enemySpawner.gameObject.SetActive(true);
        }
}