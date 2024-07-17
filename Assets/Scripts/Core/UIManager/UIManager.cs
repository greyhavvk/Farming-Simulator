using Systems.FarmingSystems;
using UnityEngine;

namespace Core.UIManager
{
    public class UIManager : MonoBehaviour, ITaskUI
    {
        [SerializeField] private PlantTaskUIPanel plantTaskUIPanel;
        [SerializeField] private GameObject moveTaskUIPanel;
        
        public void UpdateMoveTaskUI()
        {
            moveTaskUIPanel.SetActive(true);
        }

        public void UpdateTaskUI(PlantType plantType, string taskMessage)
        {
            plantTaskUIPanel.gameObject.SetActive(true);
            plantTaskUIPanel.UpdateTaskUI(plantType,taskMessage);
        }

        public void ClearTaskUI()
        {
            plantTaskUIPanel.gameObject.SetActive(false);
            moveTaskUIPanel.SetActive(false);
        }
    }
}