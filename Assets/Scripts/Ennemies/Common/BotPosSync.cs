using System;
using UnityEngine;
using UnityEngine.AI;

    public class BotPosSync : B2BComponent<BotPosSyncModel>
    {
        private NavMeshAgent _nma;
        private EnemyStatusManager _enemyStatusManager;
        private bool _spawnPositionSet;

        public Vector3 velocity;

        private void Awake()
        {
            _nma = GetComponent<NavMeshAgent>();
            _enemyStatusManager = GetComponent<EnemyStatusManager>();
        }

        protected override void OnRealtimeModelReplaced(BotPosSyncModel previousModel, BotPosSyncModel currentModel)
        {
            if (currentModel == null) return;
            if (currentModel.isFreshModel)
                currentModel.position = new Vector3();

            if (PlayersManager.current.role != PlayerRole.Shooting)
                GetPosition();
        }

        private void FixedUpdate()
        {
            if (PlayersManager.current.role == PlayerRole.Shooting)
            {
                SetPosition();
            }
            else if (PlayersManager.current.role == PlayerRole.Driving)
                GetPosition();
        }

        private void Update()
        {
            if (PlayersManager.current.role == PlayerRole.Driving)
            {
                var t = transform;

                t.position += velocity * Time.deltaTime;
                if (model.velocity != Vector3.zero)
                    t.rotation = Quaternion.Slerp(t.rotation, Quaternion.LookRotation(model.velocity),
                        Time.fixedDeltaTime * 180f);
            }
        }

        private void GetPosition()
        {
            var t = transform;

            if (Mathf.Abs(Mathf.Abs(t.position.z) - model.position.z) > 8)
            {
                t.position = model.position;
            }

            velocity = model.velocity;
        }

        private void SetPosition()
        {
            // Debug.Log($"SetPosition for enemy number {_enemyStatusManager.random}");
            model.position = transform.position;
            model.velocity = _nma.velocity;
        }
    }