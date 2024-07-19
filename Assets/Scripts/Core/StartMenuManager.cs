using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class StartMenuManager : MonoBehaviour
    {
        private const int GameSceneIndex = 1;

        public void OnStartGameButtonClicked()
        {
            SceneManager.LoadScene(GameSceneIndex);
        }
        
        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}