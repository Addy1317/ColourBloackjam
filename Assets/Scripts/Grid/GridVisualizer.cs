using UnityEngine;

namespace SlowpokeStudio.Grid
{
    [RequireComponent(typeof(LineRenderer))]
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] internal int rows = 10;
        [SerializeField] internal int columns = 10;
        [SerializeField] internal float cellSize = 1f;
        [SerializeField] internal Vector2 origin = Vector2.zero;

        [SerializeField] internal Material lineMaterial;
        [SerializeField] internal float lineWidth = 0.02f;

        private void Start()
        {
            DrawGridLines();
        }

        private void DrawGridLines()
        {
            GameObject gridParent = new GameObject("GridLines");
            gridParent.transform.SetParent(transform);

            // Draw horizontal lines
            for (int i = 0; i <= rows; i++)
            {
                Vector3 start = new Vector3(origin.x, origin.y + i * cellSize, 0);
                Vector3 end = new Vector3(origin.x + columns * cellSize, origin.y + i * cellSize, 0);
                CreateLine(start, end, gridParent.transform);
            }

            // Draw vertical lines
            for (int j = 0; j <= columns; j++)
            {
                Vector3 start = new Vector3(origin.x + j * cellSize, origin.y, 0);
                Vector3 end = new Vector3(origin.x + j * cellSize, origin.y + rows * cellSize, 0);
                CreateLine(start, end, gridParent.transform);
            }
        }

        private void CreateLine(Vector3 start, Vector3 end, Transform parent)
        {
            GameObject lineObj = new GameObject("GridLine");
            lineObj.transform.SetParent(parent);

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.useWorldSpace = true;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lr.sortingOrder = 10;
        }
    }
}
