using UnityEngine;
using UnityScriptableObjects.Runtime;
using UnityScriptableObjects.Runtime.Variables;


    [CreateAssetMenu(menuName = MenuNames.Variables + "PlayerType")]
    public class PlayerColorSo : BaseVariable, IVariable<PlayerColor>
    {
        [SerializeField] private PlayerColor value;

        public PlayerColor Value
        {
            get => value;
            set => SetProperty(ref this.value, value);
        }

        public override string ToText()
        {
            return Value.ToString();
        }
    }