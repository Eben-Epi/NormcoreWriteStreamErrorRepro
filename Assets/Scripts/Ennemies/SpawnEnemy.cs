using System;
using Normal.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ennemies
{
    public class SpawnEnemy : MonoBehaviour
    {
        private Vector3 _spawnPoint = Vector3.zero;
        private bool _oneSpawn;

        private void Start()
        {
            if (PlayersManager.current.role != PlayerRole.Shooting) return;
            InvokeRepeating(nameof(Spawn), 1, 3);
        }

        private void Spawn()
        {
            Debug.Log("Spawning Enemy");
            _spawnPoint.z = Player.instance.transform.position.z + -5;
            _spawnPoint.x = Random.Range(-13, 13);         
            var unit = B2BRealtime.Instantiate("Enemy1",
                _spawnPoint,
                Quaternion.identity, new Realtime.InstantiateOptions
                {
                    ownedByClient = true,
                    destroyWhenOwnerLeaves = false,
                    destroyWhenLastClientLeaves = true,
                    preventOwnershipTakeover = false
                });
            unit.transform.position = _spawnPoint;  
        }
    }
}