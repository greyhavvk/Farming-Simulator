using System.Collections.Generic;
using Systems.FarmingSystems;
using Systems.FinanceSystem;
using Systems.InventorySystem.InventoryItems.Data;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class SeedFarmingItem : StackableFarmingItem
    {
        public List<FarmingProgress> FarmingJobs { get; }
        public PlantType PlantType { get; }
        public int ProductID { get; }

        public SeedFarmingItem(FarmingItemData farmingItemData, ItemFinanceData itemFinanceData, int itemIndexID, Sprite icon, int maxStackCount, int initStackCount, PlantType plantType, List<FarmingProgress> farmingJobs, int productID) : base(farmingItemData, itemFinanceData, itemIndexID, icon, maxStackCount,initStackCount)
        {
            PlantType = plantType;
            FarmingJobs = new List<FarmingProgress>(farmingJobs);
            ProductID = productID;
        }
       
    }
}