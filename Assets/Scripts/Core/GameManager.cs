using Systems.PlayerSystem;
using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager.InputManager inputManager;
        [SerializeField] private PlayerController playerController;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // InputManager'ı başlat
            inputManager.Initialize();

            // PlayerController'ı başlat ve inputManager'ı ile birlikte initialize et
            playerController.Initialize(inputManager);
        }
    }
}