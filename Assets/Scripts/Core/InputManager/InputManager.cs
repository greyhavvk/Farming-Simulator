using UnityEngine;

namespace Core.InputManager
{
    public class InputManager : MonoBehaviour, IPlayerInput, IPlacementInput
    {
        [SerializeField] private InputType.InputType mouseKeyboardInput;
        private InputType.InputType _inputType;

        public void Initialize()
        {
            //TODO gerekli ayarlamalar halledilecek.
            // Varsayılan olarak UnityInputType kullanılacak
            _inputType =mouseKeyboardInput;
        }

        public void InitializeInput(InputType.InputType inputType)
        {
            _inputType = inputType;
        }

        public float GetHorizontalInput()
        {
            return _inputType.GetHorizontalInput();
        }

        public float GetVerticalInput()
        {
            return _inputType.GetVerticalInput();
        }

        public bool IsInteractButtonPressed()
        {
            return _inputType.IsInteractButtonPressed();
        }

        public float GetMouseXInput()
        {
            return _inputType.GetMouseXInput();
        }

        public float GetMouseYInput()
        {
            return _inputType.GetMouseYInput();
        }

        public bool PlacementConfirmedButtonDown()
        {
            return _inputType.IsPlacementConfirmed();
        }

        public Vector3 ConfirmedPosition()
        {
            return _inputType.ConfirmedPosition();
        }

        public bool RotateButtonDown()
        {
            return _inputType.RotateButtonDown();
        }
    }
}