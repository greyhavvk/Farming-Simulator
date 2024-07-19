using System;
using Core;
using Core.InputManager;
using Systems.TaskSystem;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerInteraction interaction;
        [SerializeField] private PlayerLookAround lookAround;
        [SerializeField] private PlayerSettings playerSettings; // PlayerSettings'i ekledik

        private IPlayerInput _playerInput;

        public void Initialize(IPlayerInput playerInput)
        {
            _playerInput = playerInput;
            lookAround.Initialize(playerSettings.TurnSensitivity); // PlayerSettings'ten dönme sensitivity'i alarak initialize ettik
            movement.Initialize(playerSettings.MoveSpeed); // PlayerSettings'ten hareket hızını alarak initialize ettik
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (_playerInput == null)
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
            var turnXInput = _playerInput.GetMouseXInput();
            var turnYInput = _playerInput.GetMouseYInput();
            lookAround.HandleLookAround(turnXInput, turnYInput);
        }

        private void HandleInteraction()
        {
            if (_playerInput.IsInteractButtonPressed())
            {
                interaction.HandleInteraction();
            }
        }

        private void HandleMovement()
        {
            var moveHorizontalInput = _playerInput.GetHorizontalInput();
            var moveVerticalInput = _playerInput.GetVerticalInput();

            movement.HandleMovement(moveHorizontalInput, moveVerticalInput);
        }

        public void TriggerTaskListener(Action<TaskData> updateTaskProgress)
        {
            movement._onUpdateTaskProgress += updateTaskProgress;
        }

        public void ClearTaskListeners()
        {
            movement._onUpdateTaskProgress =null;
        }
    }
}