using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody playerRigidbody;
        [SerializeField] private float moveSpeed=5;
        private RaycastHit _hit;
        private bool _isHit;
        private Vector3 _movementDirection;
        private Vector3 _movementAmount;
        private Vector3 _newPosition;
        private Transform _playerTransform;
        private Vector3 _playerPosition;
        

        public void Initialize(float moveSpeed)
        {
            this.moveSpeed = moveSpeed;
            _playerTransform = transform;
        }

        public void HandleMovement(float horizontalInput, float verticalInput, float deltaTime)
        {
            _movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            _movementAmount = _movementDirection * (moveSpeed * deltaTime);

            // Karakterin yeni pozisyonunu hesapla
            _playerPosition = _playerTransform.position;
            _newPosition = _playerPosition + _movementAmount;

            // Hareket ederken önümüzde engel var mı diye kontrol et
            _isHit = Physics.Raycast(_playerPosition, _movementDirection, out _hit, _movementAmount.magnitude);

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
                playerRigidbody.MovePosition(_newPosition);
            }
        }

        private void StopMovement()
        {
            // Hareketi durdurmak için karakteri yerinde tutabilirsiniz
            playerRigidbody.velocity = Vector3.zero;
        }
    }
}