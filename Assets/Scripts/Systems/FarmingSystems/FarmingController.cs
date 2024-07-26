using System;
using System.Collections.Generic;
using Systems.InventorySystem.InventoryItems;
using Systems.InventorySystem.InventoryItems.Data;
using Systems.TaskSystem;
using UnityEngine;

namespace Systems.FarmingSystems
{
    public class FarmingController : MonoBehaviour, IFieldAdded,IInteractedField
    {
        private Dictionary<GameObject,FarmingField> _fields;
        private Action<StackableFarmingItem,int> _onSeedUsed;
        private Action<TaskData> _onUpdateTaskProgress;
        private Action<int, Vector3> _onHarvestProduct;

        public void Initialize(Action<StackableFarmingItem,int> onSeedUsed,Action<int, Vector3> onHarvestProduct)
        {
            _onHarvestProduct = onHarvestProduct;
            _onSeedUsed = onSeedUsed;
            _fields = new Dictionary<GameObject, FarmingField>();
        }

        public void DisableListeners()
        {
            _onHarvestProduct = null;
            _onSeedUsed = null;
            _onHarvestProduct = null;
            foreach (var farmingField in _fields.Values)
            {
                farmingField.DisableListeners();
            }
        }

        public void AddField(GameObject fieldGameObject)
        {
            var field = fieldGameObject.GetComponent<FarmingField>();
            if (field && !_fields.ContainsValue(field))
            {
                field.Initialize(_onUpdateTaskProgress,_onHarvestProduct);
                _fields.Add(fieldGameObject,field);
            }
        }

        public void OnItemUseOnField(FarmingItemData farmingItem, GameObject interactedField)
        {
            if (_fields.ContainsKey(interactedField))
            {
                var field = _fields[interactedField];
                switch (farmingItem.FarmingItem)
                {
                    case SeedFarmingItem seedFarmingItem:
                        if (!field.TryHarvestIfReady())
                            if (field.SetCurrentSeed(seedFarmingItem))
                            {
                                if (seedFarmingItem.CurrentStackCount > 0)
                                {
                                    _onSeedUsed?.Invoke(seedFarmingItem, 1);
                                }
                            }

                        break;
                    case FarmingTool farmingTool:
                        if (!field.TryHarvestIfReady())
                        {
                            field.IncreaseJobProgress(farmingTool.FarmingJobType,
                                farmingTool.FarmingJobSpeedAdder * Time.deltaTime);
                        }
                        break;
                    default:
                        field.TryHarvestIfReady();
                        break;
                }
            }
        }

        public void TriggerTaskListener(Action<TaskData> updateTaskProgress)
        {
            _onUpdateTaskProgress += updateTaskProgress;
        }

        public void ClearTaskListeners()
        {
            _onUpdateTaskProgress = null;
        }
    }
}