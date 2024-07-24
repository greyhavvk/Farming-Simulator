using Systems.FarmingSystems;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems.SettingSO
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "InventoryItem", order = 0)]
    public class HarvestProductFarmingItemSetting:StackableInventoryItemSetting
    {
        [SerializeField] private PlantType plantType;
        
        public PlantType PlantType => plantType;
    }
}