using System;
using UnityEngine;

namespace Systems.InventorySystem
{
    [Serializable]
    public class InventoryItemData
    {
        [SerializeField] private Mesh dropItemMesh;
        [SerializeField] private Sprite icon;
        
        public Mesh DropItemMesh => dropItemMesh;
        public Sprite Icon => icon;
    }
}