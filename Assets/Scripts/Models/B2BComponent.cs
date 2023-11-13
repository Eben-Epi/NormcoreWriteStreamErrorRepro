using System;
using Normal.Realtime;
using UnityEngine;

    [RequireComponent(typeof(RealtimeView))]
    public class B2BComponent<TModel> : RealtimeComponent<TModel> where TModel : RealtimeModel, new()
    {
        private TModel _offlineModel;
        private TModel offlineModel
        {
            get
            {
                if (_offlineModel == null)
                {
                    _offlineModel = new();
                    try
                    {
                        OfflineManager.AddOfflineModel((IOfflineModel) _offlineModel);
                        OnRealtimeModelReplaced(null, _offlineModel);
                    }
                    catch (Exception e)
                    {
                        // ignored
                    }
                }

                return _offlineModel;
            }
        }
        protected new TModel model => OfflineManager.isOffline ? offlineModel : base.model;

        public double GetTime()
        {
            var time = OfflineManager.isOffline ? Time.time : room.time;

            return time;
        }
    }