using System.Collections.Generic;
using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "SeedFarmingItemSetting", menuName = "InventoryItem/SeedFarmingItemSetting", order = 0)]
    public class SeedFarmingItemSetting:StackableInventoryItemSetting
    {
        [SerializeField] private List<FarmingProgress> farmingJobs;
        [SerializeField] private PlantType plantType;
        [SerializeField] private int productID;
        
        public List<FarmingProgress> FarmingJobs => farmingJobs;
        public PlantType PlantType => plantType;
        public int ProductID => productID;
    }
}