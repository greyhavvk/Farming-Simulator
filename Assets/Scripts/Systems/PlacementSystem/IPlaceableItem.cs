using System.Collections.Generic;
using Core.Lists;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public interface IPlaceableItem
    {
        List<Vector3List> LocalPositions { get; }
        GameObject Prefab { get; }

        void Place();
        void Remove();
    }
}