using System;
using Systems.TaskSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private float moveSpeed=5;
        [SerializeField] private LayerMask layerMask;
        private RaycastHit _hit;
        private bool _isHit;
        private Vector3 _movementDirection;
        private Vector3 _movementAmount;
        private Vector3 _newPosition;
        private Transform _playerTransform;
        private Vector3 _playerPosition;
        private Vector3 _rayCastOrigin;
        
        public Action<TaskData> _onUpdateTaskProgress;

        public void Initialize(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            _playerTransform = transform;
        }

        public void HandleMovement(float horizontalInput, float verticalInput)
        {
            _movementDirection = _playerTransform.forward * verticalInput + _playerTransform.right * horizontalInput;
            _movementAmount = _movementDirection * moveSpeed ;

            // Karakterin yeni pozisyonunu hesapla
            _playerPosition = _playerTransform.position;
            _rayCastOrigin=_playerPosition+Vector3.up * .25f;
            // Hareket ederken önümüzde engel var mı diye kontrol et
            _isHit = Physics.Raycast(_rayCastOrigin, _movementDirection, out _hit, _movementAmount.magnitude* Time.deltaTime, layerMask);

            if (_isHit)
            {
                // Eğer bir şey ile çarpışacaksa, çarpışma tepkisini burada kontrol edebilirsiniz
                Debug.Log("Engel algılandı: " + _hit.collider.name);

                // Örneğin, karakteri engel öncesinde durdurabilir veya engeli atlayabilir
                StopMovement();
            }
            else
            {
                // Eğer engel yoksa, normal şekilde hareket ettir
                playerRigidbody.velocity = _movementAmount;

                if (_onUpdateTaskProgress == null) return;
                var moveTaskData = new MoveTaskData
                {
                    moveDirection = new Vector2(verticalInput, horizontalInput)
                };
                _onUpdateTaskProgress.Invoke(moveTaskData);
            }
        }

        private void StopMovement()
        {
            // Hareketi durdurmak için karakteri yerinde tutabilirsiniz
            playerRigidbody.velocity = Vector3.zero;
        }
    }
}