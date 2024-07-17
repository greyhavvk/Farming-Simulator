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

        public void Initialize(float sensitivity )
        {
            turnSensitivity = sensitivity;
            Cursor.lockState = CursorLockMode.Locked; // Fare imleci ekranın ortasında kilitlenir
        }

        public void HandleLookAround(float turnX, float turnY)
        {
            _cameraRotation -= turnY * turnSensitivity * Time.deltaTime;
            _cameraRotation = Mathf.Clamp(_cameraRotation, -90f, 90f); // Kameranın aşırı dönmesini engeller

            _playerRotation = turnX * turnSensitivity * Time.deltaTime;
            
            cameraBody.localRotation = Quaternion.Euler(_cameraRotation, 0f, 0f); // Kamerayı dikey eksende döndürür
            playerBody.Rotate(Vector3.up * _playerRotation); // Oyuncu gövdesini yatay eksende döndürür
        }
    }
}