using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UIManager
{
    public class OptionMenuUIPanel : MonoBehaviour
    {
        [SerializeField] private GameObject optionMenuOpener;
        [SerializeField] private GameObject optionPanel;

        private Action _onPauseGame;
        private Action _onResumeGame;
        
        private const int StartMenuSceneIndex=0;

        public void Initialize(Action onPauseGame, Action onResumeGame)
        {
            _onPauseGame = onPauseGame;
            _onResumeGame = onResumeGame;
        }

        public void OpenOptionMenu()
        {
            optionMenuOpener.SetActive(false);
            optionPanel.SetActive(true);
            _onPauseGame?.Invoke();
        }

        public void ReturnStartMenu()
        {
            SceneManager.LoadScene(StartMenuSceneIndex);
        }

        public void CloseOptionMenu()
        {
            _onResumeGame?.Invoke();
            optionMenuOpener.SetActive(true);
            optionPanel.SetActive(false);
        }
    }
}