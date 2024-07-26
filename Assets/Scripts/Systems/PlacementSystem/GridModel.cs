using System.Collections.Generic;
using UnityEngine;

namespace Systems.PlacementSystem
{
    public class GridModel
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly bool[,] _grid;

        public GridModel(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _grid = new bool[rows, columns];
        }

        public bool IsCellsEmpty(List<Vector2Int> cells)
        {
            foreach (var cell in cells)
            {
                if (IsWithinBounds(cell))
                {
                    if (_grid[cell.x, cell.y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void PlaceItem(List<Vector2Int> cells)
        {
            if (IsCellsEmpty(cells))
            {
                foreach (var cell in cells)
                {
                    _grid[cell.x, cell.y] = true;
                }
            }
        }

        public void RemoveItem(List<Vector2Int> cells)
        {
            foreach (var cell in cells)
            {
                if (IsWithinBounds(cell))
                {
                    _grid[cell.x, cell.y] = false;
                }
                
                
            }
        }

        private bool IsWithinBounds(Vector2Int cell)
        {
            return cell is { x: >= 0, y: >= 0 } && cell.x <= _rows && cell.y <= _columns;
        }
    }
}
