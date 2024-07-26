using System;
using System.Collections.Generic;
using Core.InputManager;
using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private List<PlaceableItem> placeableItems;
        [SerializeField] private PlaceableItem marketPlaceableItem;
        [SerializeField] private Transform marketRefPoint;
        [SerializeField] private GridController gridController;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private float placeDistance;
        [SerializeField] private Camera placementCamera;
        private IFieldAdded _fieldAdded;
        private IPlacementInput _placementInput;

        private PlaceableItem _currentItem;
        private Vector3 _currentPosition;

        private Ray _ray;
        private RaycastHit _hit;
        private Action _placementEnded;

        public void Initialize(IPlacementInput placementInput, IFieldAdded fieldAdded,Action placementEnded)
        {
            _placementEnded = placementEnded;
            _fieldAdded = fieldAdded;
            _placementInput = placementInput;
            gridController.Initialize();
            enabled = false;
            PlaceAtPoint(marketPlaceableItem, marketRefPoint.position);
        }

        private void OnDisable()
        {
            _placementEnded = null;
        }

        private void PlaceAtPoint(PlaceableItem placeableItem, Vector3 position)
        {
            var placeable = Instantiate(placeableItem);
            var nearestGridPosition = gridController.GetNearestGridCellPosition(position);
            placeable.transform.position = nearestGridPosition;
            PlaceItem(placeable);
        }


        public void StartPlacement(int placeableItemID)
        {
            _currentItem = Instantiate(placeableItems[placeableItemID]);

            _currentPosition = Vector3.zero;

            enabled = true;
        }
        
        //TODO yerleştirme iptal tuşunu eklemeliyiz.
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
            _currentItem.Place();
            if (_currentItem.gameObject.CompareTag("Field"))
            {
                _fieldAdded.AddField(_currentItem.gameObject);
            }
            _placementEnded?.Invoke();
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

            _ray = placementCamera.ScreenPointToRay(_placementInput.ConfirmedPosition());
            if (Physics.Raycast(_ray, out _hit, placeDistance, groundLayerMask))
            {
                cantFoundPosition = false;
                return _hit.point; 
            }

            cantFoundPosition = true;
            return Vector3.zero;
           
        }

        private void PlaceItem(IPlaceableItem item)
        {
            if (gridController.IsCellEmpty(item.Prefab.transform, item.LocalPositions))
            {
                gridController.PlaceItem(item.Prefab.transform, item.LocalPositions);
                item.Place();
            }
        }
    }
}
