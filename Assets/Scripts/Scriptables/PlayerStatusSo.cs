using UnityEngine;
using UnityScriptableObjects.Runtime;
using UnityScriptableObjects.Runtime.Variables;

    [CreateAssetMenu(menuName = MenuNames.Variables + "PlayerStatus")]
    public class PlayerStatusSo : BaseVariable, IVariable<PlayerRole>
    {
        [SerializeField] private PlayerRole value;

        public PlayerRole Value
        {
            get => value;
            set => SetProperty(ref this.value, value);
        }

        public override string ToText()
        {
            return Value.ToString();
        }
    }
