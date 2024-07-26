using System;
using Core.InputManager;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private float interactionDistance;
       [SerializeField] private Transform interactionRayCenter;
        private Ray _ray;
        private RaycastHit _hit;
        private Action<GameObject> _onFieldInteracted;
        private Action _onInteractingTried;
        private Action _onMarketInteracted;
        private Action _onFieldInteractionEnded;
        private Transform _currentInteractingObject;
        private bool _canInteract = true;

        private readonly String _fieldTag = "Field";

        private readonly String _marketTag = "Market";
        
        public void Initialize(Action<GameObject> onFieldInteracted, Action onInteractingTried, Action onMarketInteracted, Action onFieldInteractionEnded)
        {
            _onFieldInteractionEnded = onFieldInteractionEnded;
            _onMarketInteracted = onMarketInteracted;
            _onInteractingTried = onInteractingTried;
            _onFieldInteracted = onFieldInteracted;
        }

        public void DisableListeners()
        {
            _onFieldInteractionEnded = null;
            _onMarketInteracted = null;
            _onInteractingTried = null;
            _onFieldInteracted = null;
        }

        private void CancelInteraction()
        {
            _currentInteractingObject = null;
            _onFieldInteractionEnded?.Invoke();
        }

        private void InteractingTried()
        {
            CancelInteraction();
            _onInteractingTried?.Invoke();
        }
        
        public void HandleInteraction(IPlayerInput playerInput)
        {
            if (!_canInteract)
            {
                return;
            }
            if (playerInput.IsInteractButtonPressedDown())
            {
                _ray = new Ray(interactionRayCenter.position, interactionRayCenter.forward);
                if (Physics.Raycast(_ray, out _hit, interactionDistance, interactableLayerMask))
                {
                    _currentInteractingObject = _hit.transform;
                    if (!_currentInteractingObject.CompareTag(_fieldTag) || !_currentInteractingObject.CompareTag(_marketTag))
                    {
                        InteractingTried();
                    }
                }
                else
                { 
                    InteractingTried();
                }
            }
            else if (playerInput.IsInteractButtonPressed())
            {
                _ray = new Ray(interactionRayCenter.position, interactionRayCenter.forward);
                if (Physics.Raycast(_ray, out _hit, interactionDistance, interactableLayerMask))
                {
                    _currentInteractingObject = _hit.transform;
                    switch (_currentInteractingObject.tag)
                    {
                        case "Field":
                            _currentInteractingObject = _hit.transform; 
                            _onFieldInteracted?.Invoke(_currentInteractingObject.gameObject);
                            break;
                        case "Market":
                            _onMarketInteracted?.Invoke();
                            break;
                    }
                }
            }
            else if (playerInput.IsInteractButtonPressedUp())
            {
                CancelInteraction();
            }
        }

        public void SetUsingForOnUIStatus(bool status)
        {
            _canInteract = status;
            if (!_canInteract)
            {
                CancelInteraction();
            }
        }
    }
}