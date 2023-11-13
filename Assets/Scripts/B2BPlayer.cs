using Normal.Realtime;
using UnityEngine;

    public class B2BPlayer : B2BComponent<B2BPlayerModel>
    {
        [SerializeField] private PlayerStatusSo playerStatusSo;
        public PlayerColor debugColor;
        private GameObject _playerSyncInstance;
        private B2BPlayerModel _offlineModel;

        public bool isDriver => model.role == PlayerRole.Driving;
        public bool isShooter => model.role == PlayerRole.Shooting;
        
        public PlayerRole role
        {
            get => model.role;
            set { model.role = value; }
        }

        public bool isDoomed { get; private set; }

        public delegate void OnPlayerSwitchStateChanged();

        public OnPlayerSwitchStateChanged switchStateChanged;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            isDoomed = true;
            if (_playerSyncInstance != null)
                Destroy(_playerSyncInstance);
        }

        private void Start()
        {
            if (isOwnedLocallyInHierarchy)
            {
                if (PlayersManager.mate != null && PlayersManager.mate.role == PlayerRole.Driving)
                {
                    PlayersManager.current.role = PlayerRole.Shooting;
                    playerStatusSo.Value = PlayerRole.Shooting;
                }
                playerStatusSo.Value = role;
            }
        }

        protected override void OnRealtimeModelReplaced(B2BPlayerModel previous, B2BPlayerModel current)
        {
            if (previous != null)
            {
                previous.roleDidChange -= RoleDidChange;
            }

            if (current != null)
            {
                if (current.isFreshModel && !OfflineManager.isOffline)
                {
                    current.role = PlayerRole.Driving;
                }
                
                current.roleDidChange += RoleDidChange;
            }
        }

        private void RoleDidChange(B2BPlayerModel m, PlayerRole r)
        {
            if (isOwnedLocallyInHierarchy)
            {
                // Debug.Log("Role changed ! now it's " + r);
                playerStatusSo.Value = r;
            }
        }

        private void Update()
        {
            if (OfflineManager.isOffline)
                model.ManageOfflineCallbacks();
        }
    }