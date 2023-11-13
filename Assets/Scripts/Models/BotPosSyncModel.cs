using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

    [RealtimeModel]
    public partial class BotPosSyncModel // : RealtimeModel
    {
        [RealtimeProperty(1, false)] private Vector3 _position;
        [RealtimeProperty(2, false)] private Vector3 _velocity;
        

        //     protected override int WriteLength(StreamContext context) => MetaModelWriteLength(context) + _positionProperty.WriteLength(context);
        //
        //     protected override void Write(WriteStream stream, StreamContext context)
        //     {
        //         WriteMetaModel(stream, context);
        //         if ((0 | (_positionProperty.Write(stream, context) ? 1 : 0)) == 0)
        //             return;
        //         InvalidateContextLength(context);
        //     }
        //
        //     public UnityEngine.Vector3 position {
        //         get {
        //             return _positionProperty.value;
        //         }
        //         set {
        //             if (_positionProperty.value == value) return;
        //             _positionProperty.value = value;
        //             InvalidateReliableLength();
        //             FirePositionDidChange(value);
        //         }
        //     }
        //     
        //     public delegate void PropertyChangedHandler<in T>(BotPosSyncModel model, T value);
        //     public event PropertyChangedHandler<UnityEngine.Vector3> positionDidChange;
        //     
        //     public enum PropertyID : uint {
        //         Position = 1,
        //     }
        //     
        //     #region Properties
        //     
        //     private ReliableProperty<UnityEngine.Vector3> _positionProperty;
        //     
        //     #endregion
        //     
        //     public BotPosSyncModel() : base(new MetaModel()) {
        //         _positionProperty = new ReliableProperty<UnityEngine.Vector3>(1, _position);
        //     }
        //     
        //     protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        //         _positionProperty.UnsubscribeCallback();
        //     }
        //     
        //     private void FirePositionDidChange(UnityEngine.Vector3 value) {
        //         try {
        //             positionDidChange?.Invoke(this, value);
        //         } catch (System.Exception exception) {
        //             UnityEngine.Debug.LogException(exception);
        //         }
        //     }
        //     
        //     protected override void Read(ReadStream stream, StreamContext context) {
        //         var anyPropertiesChanged = false;
        //         while (stream.ReadNextPropertyID(out uint propertyID)) {
        //             var changed = false;
        //             switch (propertyID) {
        //                 case MetaModel.ReservedPropertyID: {
        //                     ReadMetaModel(stream, context);
        //                     break;
        //                 }
        //                 case (uint) PropertyID.Position: {
        //                     changed = _positionProperty.Read(stream, context);
        //                     if (changed) FirePositionDidChange(position);
        //                     break;
        //                 }
        //                 default: {
        //                     stream.SkipProperty();
        //                     break;
        //                 }
        //             }
        //             anyPropertiesChanged |= changed;
        //         }
        //         if (anyPropertiesChanged) {
        //             UpdateBackingFields();
        //         }
        //     }
        //     
        //     private void UpdateBackingFields() {
        //         _position = position;
        //     }
        //     
        // }
    }


