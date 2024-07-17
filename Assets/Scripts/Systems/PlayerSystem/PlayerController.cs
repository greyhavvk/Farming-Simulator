using Core;
using Core.InputManager;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerInteraction interaction;
        [SerializeField] private PlayerLookAround lookAround;
        [SerializeField] private PlayerSettings playerSettings; // PlayerSettings'i ekledik

        private IInputManager _inputManager;

        public void Initialize(IInputManager inputManager)
        {
            _inputManager = inputManager;
            lookAround.Initialize(playerSettings.TurnSensitivity); // PlayerSettings'ten dönme sensitivity'i alarak initialize ettik
            movement.Initialize(playerSettings.MoveSpeed); // PlayerSettings'ten hareket hızını alarak initialize ettik
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (_inputManager == null)
            {
                Debug.LogWarning("InputManager is not assigned.");
                return;
            }

            HandleMovement();

            HandleInteraction();

            HandleLookAround();
        }

        private void HandleLookAround()
        {
            var turnXInput = _inputManager.GetMouseXInput();
            var turnYInput = _inputManager.GetMouseYInput();
            lookAround.HandleLookAround(turnXInput, turnYInput);
        }

        private void HandleInteraction()
        {
            if (_inputManager.IsInteractButtonPressed())
            {
                interaction.HandleInteraction();
            }
        }

        private void HandleMovement()
        {
            var moveHorizontalInput = _inputManager.GetHorizontalInput();
            var moveVerticalInput = _inputManager.GetVerticalInput();

            movement.HandleMovement(moveHorizontalInput, moveVerticalInput, Time.deltaTime);
        }
    }
}