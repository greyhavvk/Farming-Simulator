using UnityEngine;

namespace Systems.PlayerSystem
{
    public class PlayerLookAround : MonoBehaviour
    {
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform cameraBody;
        [SerializeField] private float turnSensitivity = 100f;

        private float _cameraRotation = 0f;
        private float _playerRotation = 0f;
        private bool _canLookAround = true;

        public void Initialize(float sensitivity )
        {
            _cameraRotation = cameraBody.localEulerAngles.x;
            turnSensitivity = sensitivity;
            Cursor.lockState = CursorLockMode.Locked; // Fare imleci ekranın ortasında kilitlenir
        }

        public void HandleLookAround(float turnX, float turnY)
        {
            if (!_canLookAround)
            {
                return;
            }
            
            _cameraRotation -= turnY * turnSensitivity * Time.deltaTime;
            _cameraRotation = Mathf.Clamp(_cameraRotation, -45f, 45f); // Kameranın aşırı dönmesini engeller

            _playerRotation = turnX * turnSensitivity * Time.deltaTime;
            
            cameraBody.localRotation = Quaternion.Euler(_cameraRotation, 0f, 0f); // Kamerayı dikey eksende döndürür
            playerBody.Rotate(Vector3.up * _playerRotation); // Oyuncu gövdesini yatay eksende döndürür
        }

        public void SetCameraForOnUIStatus(bool status)
        {
            _canLookAround = status;
            Cursor.lockState = _canLookAround?CursorLockMode.Locked:CursorLockMode.Confined;
        }
    }
}