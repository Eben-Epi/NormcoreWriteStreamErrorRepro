using System;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;

[RealtimeModel]
public partial class B2BPlayerModel: IOfflineModel
{
    [RealtimeProperty(1, true, true)] private PlayerRole _role;

    private PlayerRole _prevRole;
    private bool _prevSwitchTrigger;

    public void ManageOfflineCallbacks()
    {
        if (_prevRole != role)
        {
            _prevRole = role;
            roleDidChange?.Invoke(this, role);
        }
    }
}


