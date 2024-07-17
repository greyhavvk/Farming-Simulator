using UnityEngine;

namespace Core.InputManager.InputType
{
    [CreateAssetMenu(fileName = "New Input Type", menuName = "Input System/Input Type")]
    public abstract class InputType : ScriptableObject
    {
        public abstract float GetHorizontalInput();
        public abstract float GetVerticalInput();
        public abstract bool IsInteractButtonPressed();
        public abstract float GetMouseXInput();
        public abstract float GetMouseYInput();
    }
}