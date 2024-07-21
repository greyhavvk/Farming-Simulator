using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class Seed : StackableFarmingItem
    {
        [SerializeField] private PlantType plantType;
        public PlantType PlantType => plantType;
    }
}