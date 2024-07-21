using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class Harvest:StackableFarmingItem
    {
        [SerializeField] private PlantType plantType;
        public PlantType PlantType => plantType;
    }
}