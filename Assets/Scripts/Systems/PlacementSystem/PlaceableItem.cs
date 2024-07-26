using System.Collections.Generic;
using Core.Lists;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public class PlaceableItem : MonoBehaviour, IPlaceableItem
    {
        [SerializeField] private List<Vector3List> localPositions;
        [SerializeField] private List<MeshRenderer> placeableMeshRenderers;
        public  List<Vector3List> LocalPositions => localPositions;
        public GameObject Prefab => gameObject;

        public void Place()
        {
            SetPlaceableColor(Color.white);
        }

        public void RotateItem()
        {
            transform.eulerAngles = transform.eulerAngles.y == 0 ? new Vector3(0, -90, 0) : Vector3.zero;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        public void CanPlace()
        {
            SetPlaceableColor(Color.green);
        }
        
        public void CantPlace()
        {
            SetPlaceableColor(Color.red);
        }

        private void SetPlaceableColor(Color color)
        {
            foreach (var placeableMeshRenderer in placeableMeshRenderers)
            {
                var materials = placeableMeshRenderer.materials;
                materials[0].color = color;
                placeableMeshRenderer.materials = materials;
            }
        }

        
    }
}