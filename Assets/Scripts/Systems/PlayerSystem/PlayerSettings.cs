using UnityEngine;

namespace Systems.PlayerSystem
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSensitivity = 100f;
        
        
        public float MoveSpeed
        {
            get => moveSpeed;
            set => moveSpeed = value;
        }

        public float TurnSensitivity
        {
            get => turnSensitivity;
            set => turnSensitivity = value;
        }
        
    }
}