using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class StartMenuManager : MonoBehaviour
    {
        private const int GameSceneIndex = 1;

        public void OnStartMenuButtonClicked()
        {
            SceneManager.LoadScene(GameSceneIndex);
        }
    }
}