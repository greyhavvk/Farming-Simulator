using System.Collections.Generic;
using UnityEngine;

namespace Systems.FarmingSystems
{
    public class PlantVisual : MonoBehaviour
    {
        [SerializeField] private PlantType plantType;
        [SerializeField] private List<GameObject> plants;

        public PlantType PlantType => plantType;
        
        public void SetPlantVisual(float progress)
        {
            foreach (var plant in plants)
            {
                plant.SetActive(false);
            }

            if (progress <= 0)
            {
                return;
            }
            else if (progress >= 1)
            {
                plants[plants.Count - 1].SetActive(true);
            }
            else
            {
                var valuePerPlant = 1f / (plants.Count - 1);
                for (int i = 0; i < plants.Count - 1; i++)
                {
                    if (progress >= i * valuePerPlant && progress < (i + 1) * valuePerPlant)
                    {
                        plants[i].SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}