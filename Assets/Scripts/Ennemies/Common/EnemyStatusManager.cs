using System;
using System.Collections;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.AI;
using UnityScriptableObjects.Runtime.Variables;
using Random = UnityEngine.Random;

public class EnemyStatusManager : B2BComponent<EnemyStatusModel>
    {
        public delegate void EnemyDied(int segmentId, float random_id);

        public static EnemyDied onEnemyDied;
        public bool isVisibleBeforeActivation;
        public EnemyStatus status;
        [SerializeField] private GameObject deathVfx;
        [SerializeField] private Vector2 delayRange;
        public float activationDistance;
        private Transform _driver;
        public bool playDeathVfx;
        private bool _activateSent;
        [SerializeField] private Collider coll;
        [SerializeField] private MeshRenderer[] _meshRenderers;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderers;
        [SerializeField] private FloatVariable playerBaseSpeed;
        private RealtimeView _realtimeView;
        private PlayerColor color;
        private bool isDoomed = false;
        private bool willDie = false;
        public float random;
        [SerializeField] private NavMeshAgent _nma;

        protected virtual void Start()
        {
            random = Random.Range(0f, 10000f);
            Debug.Log($"Ennemy Creation {random}");
            gameObject.name = "robot_" + random;
            _realtimeView = GetComponent<RealtimeView>();
            model.activationDelay = Random.Range(delayRange.x, delayRange.y);
            if (gameObject.name == "Enemy1_Pink(Clone)")
            {
                color = PlayerColor.Pink;
            }
            else
            {
                color = PlayerColor.Blue;
            }
        }

        protected virtual void OnEnable()
        {
            _activateSent = false;
            _meshRenderers = GetComponentsInChildren<MeshRenderer>();
            _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            playDeathVfx = true;
            status = EnemyStatus.Idle;
            if (!isVisibleBeforeActivation)
                SetInteractivity(false);
            if (Player.instance != null)
                _driver = Player.instance.transform;
        }

        private void SetInteractivity(bool interactive)
        {
            coll.enabled = interactive;
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.gameObject.SetActive(interactive);
            }

            foreach (var smr in _skinnedMeshRenderers)
            {
                smr.gameObject.SetActive(interactive);
            }

        }

        public void Die()
        {
            Debug.Log($"Die in enemystatusManaager {random}");
            if (isDoomed)
            {
                Debug.Log($"esm isdoomed return {random}");
                return;
            }

            if (PlayersManager.current.role != PlayerRole.Shooting)
                return;
            if (status == EnemyStatus.Dead) return;

            isDoomed = true;
            status = EnemyStatus.Dead;
            var t = transform;
            if (deathVfx != null && playDeathVfx)
                Instantiate(deathVfx, t.position, t.rotation);
            // status = EnemyStatus.Idle;
            playDeathVfx = false;
            if (!isVisibleBeforeActivation)
                SetInteractivity(false);
            if (OfflineManager.isOffline || isOwnedLocallyInHierarchy)
            {
                Debug.Log($"isOwnedLocally in esm : DESTROYING ID NO {random}");
                // Debug.Log("Enemy destroyed");
                B2BRealtime.Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Debug.Log($"OnDestroy ESM ? {random}");
            isDoomed = true;
            if (deathVfx != null && playDeathVfx)
            {
                var t = transform;
                Instantiate(deathVfx, t.position, t.rotation);
            }
        }

        private IEnumerator ActivateCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!isVisibleBeforeActivation)
                SetInteractivity(true);
            status = EnemyStatus.Active;
        }

        protected virtual void Update()
        {
            
            if (_driver != null && status == EnemyStatus.Idle &&
                _driver.position.z > transform.position.z + activationDistance &&
                !_activateSent)
            {
                StartCoroutine(ActivateCoroutine(model.activationDelay));
                _activateSent = true;
                _nma.enabled = true;
                _nma.speed = playerBaseSpeed.Value * .5f;
                _nma.Warp(transform.position);
            }
            if (PlayersManager.current.role != PlayerRole.Shooting) return;
            SetDestination(GetTarget());
            if (!willDie)
            {
                willDie = true;
                StartCoroutine(GonnaDieCoroutine());
            }
        }

        private Vector3 GetTarget()
        {
            if (_driver == null)
                return transform.position;
            var tp = _driver.position;
            if (transform.position.z < tp.z)
                tp.z += 20;
            return tp;
        }
        
        private void SetDestination(Vector3 destination)
        {
            if (_nma.isOnNavMesh)
                _nma.SetDestination(destination);
        }
        
        private IEnumerator GonnaDieCoroutine()
        {
            Debug.Log($"Ennemy {random} dying");
            yield return new WaitForSeconds(2);
            Die();
        }
    }