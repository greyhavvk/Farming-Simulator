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
        private Transform _currentInteractingObject;
        
        public void Initialize(Action<GameObject> onFieldInteracted, Action onInteractingTried, Action onMarketInteracted)
        {
            _onMarketInteracted = onMarketInteracted;
            _onInteractingTried = onInteractingTried;
            _onFieldInteracted = onFieldInteracted;
        }

        public void CancelInteraction()
        {
            _currentInteractingObject = null;
        }
        
        public void HandleInteraction(IPlayerInput playerInput)
        {
            if (playerInput.IsInteractButtonPressed())
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
                        case "Shop":
                            _onMarketInteracted?.Invoke();
                            break;
                        default:
                            CancelInteraction();
                            _onInteractingTried?.Invoke();
                            break;
                    }
                }
                else
                {
                    _onInteractingTried?.Invoke();
                    CancelInteraction();
                    // RANDOM TIKLAMA OLMUŞ. ONA GÖRE İŞLEM YÜRÜT.
                }
            
            }
            else if (playerInput.IsInteractButtonPressedUp())
            {
                CancelInteraction();
            }
        }
    }
}