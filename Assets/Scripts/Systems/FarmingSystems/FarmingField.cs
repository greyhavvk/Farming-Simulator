using System;
using System.Collections.Generic;
using Core;
using Systems.InventorySystem.InventoryItems;
using Systems.TaskSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.FarmingSystems
{
    public class FarmingField : MonoBehaviour
    {
        [SerializeField] private List<PlantVisual> plantVisuals;
        [SerializeField] private Slider progressBar;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text text;
        [SerializeField] private GameObject progressParent;
        [SerializeField] private Transform harvestSpawnPoint;
        private List<FarmingProgress> _farmingJobs;
        private SeedFarmingItem _currentSeed;
        private bool _isPlanted;
        private bool _readyToHarvest;
        private Action<TaskData> _onUpdateTaskProgress;
        private Action<int, Vector3> _onHarvestProduct;


        public void Initialize(Action<TaskData> onUpdateTaskProgress,  Action<int, Vector3> onHarvestProduct)
        {
            _onHarvestProduct = onHarvestProduct;
            _onUpdateTaskProgress = onUpdateTaskProgress;
            _currentSeed = null;
            _isPlanted = false;
            _readyToHarvest = false;
            enabled = false;
        }

        public void DisableListeners()
        {
            _onHarvestProduct = null;
            _onUpdateTaskProgress = null;
        }

        public bool SetCurrentSeed(SeedFarmingItem seedFarmingItem)
        {
            if (_currentSeed == null)
            {
                _farmingJobs = new List<FarmingProgress>();
                _currentSeed = seedFarmingItem;
                for (int i = 0; i < _currentSeed.FarmingJobs.Count; i++)
                {
                    var farmingJob = DeepClone.DeepCloneIt(_currentSeed.FarmingJobs[i]);
                    _farmingJobs.Add(farmingJob);
                }
                UpdateVisual();
                var plantSeedTask = new PlantSeedData()
                {
                    plantType = _currentSeed.PlantType
                };
                _onUpdateTaskProgress?.Invoke(plantSeedTask);
                return true;
            }

            return false;
        }

        private void UpdateVisual()
        {
            if (_currentSeed != null)
            {
                if (_farmingJobs.Count>0)
                {
                    progressParent.SetActive(true);
                    progressBar.value = _farmingJobs[0].JobCompletionRate;
                    text.text = _currentSeed.PlantType + " " + _farmingJobs[0].farmingJobType;
                    icon.sprite = _currentSeed.Icon;
                }
                else
                {
                    progressParent.SetActive(false);
                }
                SetPlantVisual();
            }
            else
            {
                progressParent.SetActive(false);
            }
        }

        private void SetPlantVisual()
        {
            foreach (var plantVisual in plantVisuals)
            {
                plantVisual.gameObject.SetActive(false);
            }

            if (_readyToHarvest)
            {
                var plant = plantVisuals.Find(match: visual => visual.PlantType == _currentSeed.PlantType);
                if (plant)
                {
                    plant.gameObject.SetActive(true);
                    plant.SetPlantVisual(1);
                }
                return;
            }
            if (_isPlanted)
            {
                var plant = plantVisuals.Find(match: visual => visual.PlantType == _currentSeed.PlantType);
                if (plant)
                {
                    plant.gameObject.SetActive(true);
                    plant.SetPlantVisual(_farmingJobs[0].JobCompletionRate);
                }
            }
        }

        public void IncreaseJobProgress(FarmingJobType farmingJobType, float progressAdder)
        {
            if (_farmingJobs.Count==0)
            {
                return;
            }
            if (farmingJobType==_farmingJobs[0].farmingJobType)
            {
                _farmingJobs[0].currentCompletionValue += progressAdder;
                if (_farmingJobs[0].JobCompletionRate>=1)
                {
                    _farmingJobs.Remove(_farmingJobs[0]);
                    if (_farmingJobs.Count>0)
                    {
                        if (_farmingJobs[0].farmingJobType==FarmingJobType.Growing)
                        {
                            _isPlanted = true;
                            enabled = true;
                        }
                    }
                    else
                    {
                        _readyToHarvest = true;
                    }
                }
                
                UpdateVisual();
            }
        }

        public void Update()
        {
            if (_isPlanted && !_readyToHarvest)
            {
                IncreaseJobProgress(FarmingJobType.Growing,
                    _farmingJobs[0].targetCompletionSpeedValue * Time.deltaTime);
            }
        }

        private void Harvest()
        {
            foreach (var plantVisual in plantVisuals)
            {
                plantVisual.gameObject.SetActive(false);
            }

            var harvestTaskData = new HarvestTaskData()
            {
                plantType = _currentSeed.PlantType
            };
            _onUpdateTaskProgress.Invoke(harvestTaskData);
            _onHarvestProduct.Invoke(_currentSeed.ProductID, harvestSpawnPoint.position);
            _currentSeed = null;
            _isPlanted = false;
            _readyToHarvest = false;
            _farmingJobs.Clear();
            enabled = false;
            UpdateVisual();
        }

        public bool TryHarvestIfReady()
        {
            if (_readyToHarvest)
            {
                Harvest();
                return true;
            }

            return false;
        }
    }
}