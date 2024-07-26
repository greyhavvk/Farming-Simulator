using System.Collections.Generic;
using Core.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Systems.InventorySystem.InventoryItems
{
    public class DropItem : PoolableObject
    {
        [SerializeField] private List<GameObject> dropItemVisuals;
        [SerializeField] private TMP_Text stackText;
        [SerializeField] private Collider dropItemCollider;
        private readonly float _minRadius = 1.0f; 
        private readonly float _maxRadius = 3.0f;
        private readonly float _jumpPower = 1.0f;
        private readonly int _numJumps = 1; 
        private readonly float _dropDuration = .5f; 

        private IFarmingItem _farmingItem;

        public IFarmingItem FarmingItem => _farmingItem;

        public void Drop(Vector3 dropCenter)
        {
            dropItemCollider.enabled = false;
            float angle = Random.Range(-Mathf.PI / 2, Mathf.PI / 2);
            float radius = Random.Range(_minRadius, _maxRadius);

            Vector3 targetPosition = dropCenter + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            transform.position = dropCenter;
            transform.DOJump(targetPosition, _jumpPower, _numJumps, _dropDuration).OnComplete(() =>
            {
                dropItemCollider.enabled = true;
            });
        }

    public void Collect()
        {
            dropItemCollider.enabled = false;
            base.ReturnToPool();
        }

        public void UpdateFarmingItem(IFarmingItem item)
        {
            _farmingItem = item;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_farmingItem==null)
            {
                gameObject.SetActive(false);
                return;
            }

            for (int i = 0; i < dropItemVisuals.Count; i++)
            {
                dropItemVisuals[i].SetActive(i==_farmingItem.ItemIndexID);
            }
            
            StackableFarmingItem stackableFarmingItem=_farmingItem as StackableFarmingItem;
            if (stackableFarmingItem!=null)
            {
                stackText.text = stackableFarmingItem.CurrentStackCount.ToString();
            }
            stackText.gameObject.SetActive(stackableFarmingItem!=null);

        }

        public void Spawn(Vector3 position)
        {
            transform.position = position;
            dropItemCollider.enabled = true;
        }
    }
}