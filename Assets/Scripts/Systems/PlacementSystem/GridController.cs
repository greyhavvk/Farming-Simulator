using System.Collections.Generic;
using Core.Lists;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private float cellSize;

        private GridModel _gridModel;
        private Vector3 _gridOrigin;
        
        public void Initialize()
        {
            _gridModel = new GridModel(rows, columns);
            _gridOrigin = CalculateGridOrigin();
        }

        private Vector3 CalculateGridOrigin()
        {
            var gridWidth = columns * cellSize;
            var gridHeight = rows * cellSize;
            return new Vector3(-gridWidth / 2, 0, -gridHeight / 2);
        }

        private List<Vector2Int> CalculateCells(Transform itemTransform, List<Vector3List> localPositions)
        {
            var width = Mathf.CeilToInt(localPositions[0].vector3List.Count / cellSize);
            var height = Mathf.CeilToInt(localPositions.Count / cellSize);

            var cells = new List<Vector2Int>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var localPositionToGlobalPosition = itemTransform.TransformPoint(localPositions[i].vector3List[j]) - _gridOrigin;
                    var row = Mathf.FloorToInt(localPositionToGlobalPosition.x / cellSize);
                    var column = Mathf.FloorToInt(localPositionToGlobalPosition.z / cellSize);
                    var cell = new Vector2Int(row, column);
                    cells.Add(cell);
                }
            }

            return cells;
        }

        public bool IsCellEmpty(Transform itemTransform, List<Vector3List> localPositions)
        {
            var cells = CalculateCells(itemTransform, localPositions);

            return cells.Count>0 && _gridModel.IsCellsEmpty(cells);
        }

        public void PlaceItem(Transform itemTransform, List<Vector3List> localPositions)
        {
            var cells = CalculateCells(itemTransform, localPositions);

            _gridModel.PlaceItem(cells);
        }

        public void RemoveItem(Transform itemTransform, List<Vector3List> localPositions)
        {
            var cells = CalculateCells(itemTransform, localPositions);

            if (cells.Count>0)
            {
                _gridModel.RemoveItem(cells);
            }
        }

        public Vector3 GetNearestGridCellPosition(Vector3 worldPosition)
        {
            var localPosition = worldPosition - CalculateGridOrigin();

            var row = Mathf.FloorToInt(localPosition.x / cellSize);
            var column = Mathf.FloorToInt(localPosition.z / cellSize);

            return CalculateGridOrigin() + new Vector3(row * cellSize, 0, column * cellSize);
        }
    }
}
