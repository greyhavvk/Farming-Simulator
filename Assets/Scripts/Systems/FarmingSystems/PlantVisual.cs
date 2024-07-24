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
            // Tüm bitkileri devre dışı bırak
            foreach (var plant in plants)
            {
                plant.SetActive(false);
            }

            if (progress <= 0)
            {
                // Progress 0 olduğunda tüm bitkiler devre dışı bırakılmış olacak
                return;
            }
            else if (progress >= 1)
            {
                // Progress 1 olduğunda sadece son bitki aktif olacak
                plants[plants.Count - 1].SetActive(true);
            }
            else
            {
                // Progress 0 ve 1 arasında olduğunda uygun bitkiyi aktif et
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