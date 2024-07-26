using System;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerDetectionHandler : MonoBehaviour
    {
        private readonly string _dropItemTag = "DropItem";
        private Action<GameObject> _onDropItemDetected;

        public void Initialize(Action<GameObject> onDropItemDetected)
        {
            _onDropItemDetected = onDropItemDetected;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_dropItemTag))
            {
                _onDropItemDetected?.Invoke(other.gameObject);
            }
        }
    }
}