using Normal.Realtime;
using Normal.Realtime.Serialization;


    [RealtimeModel]
    public partial class StringModel
    {
        [RealtimeProperty(1, true, true)] private string _value;
    }



