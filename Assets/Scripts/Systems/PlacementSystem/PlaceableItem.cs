using System.Collections.Generic;
using Lists;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public class PlaceableItem : MonoBehaviour, IPlaceableItem
    {
        [SerializeField] private List<Vector3List> localPositions; // Öğenin boyutu
        [SerializeField] private GameObject prefab; // Öğenin prefab'ı
        [SerializeField] private MeshRenderer placeableMeshRenderer;
        public  List<Vector3List> LocalPositions => localPositions;
        public GameObject Prefab => prefab;

        public void Place(Vector3 position)
        {
            SetPlaceableColor(Color.white);
            transform.position = position;
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
            var materials = placeableMeshRenderer.materials;
            materials[0].color = color;
            placeableMeshRenderer.materials = materials;
        }

        
    }
}