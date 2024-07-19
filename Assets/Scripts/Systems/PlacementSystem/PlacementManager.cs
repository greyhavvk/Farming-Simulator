using Core.InputManager;
using UnityEditor;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private PlaceableItem testObject;
        [SerializeField] private GridController gridController;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float placeDistance;
        [SerializeField] private Camera placementCamera;
        private IPlacementInput _placementInput;

        private PlaceableItem _currentItem;
        private Vector3 _currentPosition;

        private Ray _ray;
        private RaycastHit _hit;

        public void Initialize(IPlacementInput placementInput)
        {
            _placementInput = placementInput;
            gridController.Initialize();
            enabled = false;
        }

        public void StartPlacement(IPlaceableItem placeableItem)
        {
            if (_currentItem != null)
            {
                Destroy(_currentItem.Prefab);
            }

            var newPlaceableItem = Instantiate(placeableItem.Prefab);
            _currentItem = newPlaceableItem.GetComponent<PlaceableItem>();

            _currentPosition = Vector3.zero;

            enabled = true;
        }

        [ContextMenu("Place")]
        public void TestPlacement()
        {
            StartPlacement(testObject);
        }

        [ContextMenu("Cancel")]
        public void CancelPlacement()
        {
            if (_currentItem)
            {
                Destroy(_currentItem.Prefab);
                _currentItem = null;
            }

            enabled = false;
        }

        private void Update()
        {
            if (_currentItem)
            {
                HandlePlacement();
            }
        }

        private void HandlePlacement()
        {
            HandleRotation();
            UpdateItemPosition(out var cantFoundPosition);
            CheckPlacementValidity(cantFoundPosition);

            if (cantFoundPosition)
            {
                return;
            }
            
            if (_placementInput.PlacementConfirmedButtonDown())
            {
                PlaceCurrentItem();
            }
        }

        private void HandleRotation()
        {
            if (_placementInput.RotateButtonDown())
            {
                _currentItem.RotateItem();
            }
        }

        private void UpdateItemPosition(out bool cantFoundPosition)
        {
            
            var focusedPointToWorldPosition = GetFocusedPointToWorldPosition(out cantFoundPosition);
            _currentPosition = gridController.GetNearestGridCellPosition(focusedPointToWorldPosition);
            _currentItem.transform.position = _currentPosition;
        }

        private void CheckPlacementValidity(bool cantFoundPosition)
        {
            _currentItem.Prefab.SetActive(!cantFoundPosition);
            
            if (gridController.IsCellEmpty(_currentItem.Prefab.transform, _currentItem.LocalPositions))
            {
                _currentItem.CanPlace();
            }
            else
            {
                _currentItem.CantPlace();
            }
        }

        private void PlaceCurrentItem()
        {
            PlaceItem(_currentItem);
            _currentItem.Place(_currentPosition);
            _currentItem = null;
            enabled = false;
        }

        private Vector3 GetFocusedPointToWorldPosition(out bool cantFoundPosition)
        {
            if (!placementCamera)
            {
                cantFoundPosition = true;
                return Vector3.zero;
            }

            _ray = placementCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, placeDistance, groundLayerMask))
            {
                cantFoundPosition = false;
                return _hit.point; 
            }

            cantFoundPosition = true;
            return Vector3.zero;
           
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void PlaceItem(IPlaceableItem item)
        {
            if (gridController.IsCellEmpty(item.Prefab.transform, item.LocalPositions))
            {
                var position = gridController.PlaceItem(item.Prefab.transform, item.LocalPositions);
                item.Place(position);
            }
            else
            {
                // Log or handle the case where placement fails
                Debug.LogWarning("Cell is not empty or cannot place the item.");
            }
        }
    }
}
