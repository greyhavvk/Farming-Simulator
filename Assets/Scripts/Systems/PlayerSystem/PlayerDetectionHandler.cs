using System;
using UnityEngine;

namespace Core.Player
{
    public class PlayerDetectionHandler : MonoBehaviour
    {
        [SerializeField] private string dropItemTag = "DropItem";
        private Action<GameObject> _onDropItemDetected;

        public void Initialize(Action<GameObject> onDropItemDetected)
        {
            _onDropItemDetected = onDropItemDetected;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(dropItemTag))
            {
                _onDropItemDetected?.Invoke(other.gameObject);
            }
        }
    }
}