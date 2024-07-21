using System;
using UnityEngine;

namespace Systems.InventorySystem
{
    [Serializable]
    public class FarmingItemData:InventoryItemData
    {
        public IFarmingItem FarmingItem { get; set; }
    }
}