using System;
using System.Collections.Generic;
using Systems.FarmingSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UIManager
{
    [Serializable]
    public class PlantIcon
    {
        public PlantType plantType;
        public Sprite sprite;
    }
    
    public class PlantTaskUIPanel : MonoBehaviour
    {
        [SerializeField] private Image plantIcon;
        [SerializeField] private TMP_Text progress;
        [SerializeField] private List<PlantIcon> plantIcons;


        public void UpdateTaskUI(PlantType plantType, string taskMessage)
        {
            plantIcon.sprite = plantIcons.Find(icon => icon.plantType == plantType).sprite;

            progress.text = taskMessage;
        }
    }
}