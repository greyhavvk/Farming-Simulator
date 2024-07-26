using System;
using Core.InputManager;
using Core.UIManager.Inventory;
using Systems.FarmingSystems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.TaskSystem;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerInteraction interaction;
        [SerializeField] private PlayerLookAround lookAround;
        [SerializeField] private PlayerSettings playerSettings; 
        [SerializeField] private PlayerHoldingItemHandler holdingItemHandler;
        [SerializeField] private PlayerHotBarHandler hotBarHandler;
        [SerializeField] private PlayerDetectionHandler detectionHandler;
        private IPlayerInput _playerInput;

        public void Initialize(IPlayerInput playerInput, IInventoryUI inventoryUI, Action<GameObject> onDropItemDetected, IInteractedField interactedField, Action onMarketInteracted)
        {
            interaction.Initialize(FieldInteracted, InteractingTried, onMarketInteracted, FieldInteractionEnded);
            detectionHandler.Initialize(onDropItemDetected);
            _playerInput = playerInput;
            lookAround.Initialize(playerSettings.TurnSensitivity);
            movement.Initialize(playerSettings.MoveSpeed); 
            hotBarHandler.Initialize(inventoryUI, holdingItemHandler.SetHoldingItem);
            holdingItemHandler.Initialize(interactedField);
        }

        private void SetPlayerMoveAndCameraForOnUIStatus(bool status)
        {
            movement.SetPlayerMoveForOnUIStatus(status);
            lookAround.SetCameraForOnUIStatus(status);
            interaction.SetUsingForOnUIStatus(status);
        }

        private void FieldInteractionEnded()
        {
            holdingItemHandler.FieldInteractionEnded();
        }

        private void InteractingTried()
        {
            holdingItemHandler.InteractingTriedAnimation();
        }

        private void FieldInteracted(GameObject obj)
        {
            holdingItemHandler.FieldInteracted(obj);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (_playerInput == null)
            {
                return;
            }

            HandleMovement();

            HandleInteraction();

            HandleLookAround();

            HandleItemSelectScroll();
        }

        private void HandleLookAround()
        {
            var turnXInput = _playerInput.GetMouseXInput();
            var turnYInput = _playerInput.GetMouseYInput();
            lookAround.HandleLookAround(turnXInput, turnYInput);
        }

        private void HandleInteraction()
        {
           interaction.HandleInteraction(_playerInput);
        }

        private void HandleMovement()
        {
            var moveHorizontalInput = _playerInput.GetHorizontalInput();
            var moveVerticalInput = _playerInput.GetVerticalInput();

            movement.HandleMovement(moveHorizontalInput, moveVerticalInput);
        }

        public void TriggerTaskListener(Action<TaskData> updateTaskProgress)
        {
            movement.onUpdateTaskProgress += updateTaskProgress;
        }

        public void ClearTaskListeners()
        {
            movement.onUpdateTaskProgress =null;
        }
        
        public void RefreshHotBar()
        {
            hotBarHandler.RefreshHotBar();
        }

        public void RemoveItemFromHotBar(FarmingItemData itemData)
        {
            hotBarHandler.RemoveItemFromHotBar(itemData);
        }
        
        private void HandleItemSelectScroll()
        {
            var scrollDelta = _playerInput.GetScrollDelta();
            
                hotBarHandler.HandleItemSelectScroll(scrollDelta);
        }

        public void LoseControls()
        {
            SetPlayerMoveAndCameraForOnUIStatus(false);
        }

        public void GainControls()
        {
            SetPlayerMoveAndCameraForOnUIStatus(true);
        }
    }
}