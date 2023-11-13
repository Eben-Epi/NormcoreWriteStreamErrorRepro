using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class EnemyStatusModel: IOfflineModel
{
    [RealtimeProperty(1, true, true)] private EnemyStatus _enemyStatus;
    [RealtimeProperty(2, true)] private float _activationDelay;

    private EnemyStatus _prevEnemyStatus;
    
    public void ManageOfflineCallbacks()
    {
        if (_prevEnemyStatus != enemyStatus)
        {
            _prevEnemyStatus = enemyStatus;
            enemyStatusDidChange?.Invoke(this, enemyStatus);
        }
    }
}


