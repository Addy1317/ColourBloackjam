using UnityEngine;

namespace SlowpokeStudio.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        public int rows = 10;
        public int columns = 10;
        public float cellSize = 1f;
        public Vector2 originPosition = Vector2.zero;

        [Header("Debug Settings")]
        public bool showGizmos = true;
        public Color gridColor = Color.gray;
        public Color occupiedColor = Color.red;

        private bool[,] gridData;

        private void Awake()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            gridData = new bool[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    gridData[row, col] = false;
                }
            }
        }

        public Vector2Int GetGridPosition(Vector2 worldPosition)
        {
            int col = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
            int row = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
            return new Vector2Int(row, col);
        }

        public Vector2 GetWorldPosition(int row, int col)
        {
            float x = originPosition.x + (col * cellSize) + cellSize / 2f;
            float y = originPosition.y + (row * cellSize) + cellSize / 2f;
            return new Vector2(x, y);
        }

        public bool IsCellOccupied(int row, int col)
        {
            if (!IsInBounds(row, col)) return true;
            return gridData[row, col];
        }

        public void SetCell(int row, int col, bool occupied)
        {
            if (IsInBounds(row, col))
            {
                gridData[row, col] = occupied;
            }
        }

        public bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < rows && col >= 0 && col < columns;
        }

        private void OnDrawGizmos()
        {
            if (!showGizmos || gridData == null)
                return;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Vector2 cellCenter = GetWorldPosition(row, col);
                    Gizmos.color = gridData[row, col] ? occupiedColor : gridColor;
                    Gizmos.DrawWireCube(cellCenter, Vector3.one * cellSize * 0.95f);
                }
            }
        }
    }
}
