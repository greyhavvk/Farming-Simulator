using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "HarvestProductFarmingItemSetting", menuName = "InventoryItem/HarvestProductFarmingItemSetting", order = 0)]
    public class HarvestProductFarmingItemSetting:StackableInventoryItemSetting
    {
        [SerializeField] private PlantType plantType;
        
        public PlantType PlantType => plantType;
    }
}