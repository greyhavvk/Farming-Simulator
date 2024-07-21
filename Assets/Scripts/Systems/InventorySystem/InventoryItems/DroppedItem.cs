using System.Collections.Generic;
using Core.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Systems.InventorySystem
{
    public class DropItem : PoolableObject
    {
        [SerializeField] private List<GameObject> dropItemVisuals;
        [SerializeField] private TMP_Text stackText;
        [SerializeField] private Collider dropItemCollider;
        private float _minRadius = 1.0f; // Minimum yarıçap
        private float _maxRadius = 3.0f; // Maksimum yarıçap
        private float _jumpPower = 1.0f; // Zıplama gücü
        private int _numJumps = 1; // Zıplama sayısı
        private float _dropDuration = .5f; // Zıplama

        private IFarmingItem _farmingItem;

        public IFarmingItem FarmingItem => _farmingItem;

        public void Drop(Vector3 dropCenter)
        {
            dropItemCollider.enabled = false;
            float angle = Random.Range(0, 2 * Mathf.PI);
            float radius = Random.Range(_minRadius, _maxRadius);

            // Hedef pozisyonu hesapla
            Vector3 targetPosition = dropCenter + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            transform.position = dropCenter;
            // DOTween Jump fonksiyonunu kullanarak objeyi hedef pozisyona zıplat
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
                dropItemVisuals[i].SetActive(i==_farmingItem.DropItemIndex);
            }
            
            StackableFarmingItem stackableFarmingItem=_farmingItem as StackableFarmingItem;
            if (stackableFarmingItem)
            {
                stackText.text = stackableFarmingItem.CurrentStackCount.ToString();
            }
            stackText.gameObject.SetActive(stackableFarmingItem);

        }
    }
}