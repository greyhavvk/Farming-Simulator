using System;
using Systems.TaskSystem;
using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody playerRigidbody;
        private float _moveSpeed;
        [SerializeField] private LayerMask layerMask;
        private bool _isHit;
        private Vector3 _movementDirection;
        private Vector3 _movementAmount;
        private Vector3 _newPosition;
        private Transform _playerTransform;
        private Vector3 _playerPosition;
        private Vector3 _rayCastOrigin;
        private bool _canMove = true;
        public Action<TaskData> onUpdateTaskProgress;

        public void Initialize(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
            _playerTransform = transform;
        }

        public void DisableListeners()
        {
            onUpdateTaskProgress = null;
        }

        public void HandleMovement(float horizontalInput, float verticalInput)
        {  
            if (!_canMove)
            {
                return;
            }
            _movementDirection = _playerTransform.forward * verticalInput + _playerTransform.right * horizontalInput;
            _movementAmount = _movementDirection * _moveSpeed ;
            _playerPosition = _playerTransform.position;
            _rayCastOrigin=_playerPosition+Vector3.up * .25f;
            _isHit = Physics.Raycast(_rayCastOrigin, _movementDirection, _movementAmount.magnitude* Time.deltaTime, layerMask);

            if (_isHit)
            {
                StopMovement();
            }
            else
            {
                playerRigidbody.velocity = _movementAmount;
                if (onUpdateTaskProgress == null) return;
                var moveTaskData = new MoveTaskData
                {
                    moveDirection = new Vector2(verticalInput, horizontalInput)
                };
                onUpdateTaskProgress.Invoke(moveTaskData);
            }
        }

        private void StopMovement()
        {
            playerRigidbody.velocity = Vector3.zero;
        }

        public void SetPlayerMoveForOnUIStatus(bool status)
        {
            _canMove = status;
            StopMovement();
        }
    }
}