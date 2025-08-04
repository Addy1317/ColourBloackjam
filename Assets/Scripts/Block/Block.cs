using SlowpokeStudio.Grid;
using UnityEngine;

namespace SlowpokeStudio.ColourBlocks
{
    public class Block : MonoBehaviour
    {
        [Header("Injected Data")]
        [SerializeField] internal BlockColor blockColor;
        [SerializeField] internal Vector2Int gridOrigin;
        [SerializeField] internal Vector2Int[] shapeOffsets;

        private GridManager gridManager;

        internal void Initialize(BlockColor color, Vector2Int origin, Vector2Int[] offsets, GridManager gridRef)
        {
            blockColor = color;
            gridOrigin = origin;
            shapeOffsets = offsets;
            gridManager = gridRef;

            Debug.Log($"[Block] Initialized at {gridOrigin} with shape {offsets.Length} tiles.");
        }

        private int GetWallIndexFromExit(Vector2Int exitCell)
        {
            int maxRows = gridManager.rows;
            int maxCols = gridManager.columns;

            // Determine wall index based on exit direction
            if (exitCell.y < 0) return 0;                     // Bottom wall
            if (exitCell.x >= maxCols) return 2;              // Right wall
            if (exitCell.y >= maxRows) return 4;              // Top wall
            if (exitCell.x < 0) return 6;                     // Left wall

            Debug.LogWarning("[Block] Exit direction invalid. Defaulting to wall index 0.");
            return 0; // Fallback
        }

        internal void PrepareForMove()
        {
            foreach (var offset in shapeOffsets)
            {
                Vector2Int pos = gridOrigin + offset;
                gridManager.SetCell(pos.x, pos.y, false); // Free cell
            }
        }
        
        internal bool CanMoveTo(Vector2Int newOrigin)
        {
            foreach (var offset in shapeOffsets)
            {
                var pos = newOrigin + offset;
                if (!gridManager.IsInBounds(pos.x, pos.y) || gridManager.IsCellOccupied(pos.x, pos.y))
                {
                    Debug.LogWarning($"[Block] Cannot move to {pos} — Out of bounds or occupied");
                    return false;
                }
            }
            return true;
        }

        internal void MoveTo(Vector2Int newOrigin)
        {
            gridOrigin = newOrigin;
            transform.position = gridManager.GetWorldPosition(gridOrigin);
        }

        internal void ReleaseBlock()
        {
            foreach (var offset in shapeOffsets)
            {
                Vector2Int pos = gridOrigin + offset;
                gridManager.SetCell(pos.x, pos.y, true); // Occupy cell
                Debug.DrawRay(gridManager.GetWorldPosition(pos), Vector3.up * 0.3f, Color.green, 1f);
            }
        }

        public void CheckWallExit()
        {
            Vector2Int exitCell = gridOrigin;

            if (!gridManager.IsInBounds(exitCell.x, exitCell.y))
            {
                Debug.Log($"[Block] {blockColor} block EXITED grid at {exitCell}");

                int wallIndex = GetWallIndexFromExit(exitCell);
                Debug.Log($"[Block] Exit corresponds to Wall Index: {wallIndex}");

                BlockColor wallColor = GameService.Instance.wallManager.GetWallColor(wallIndex);
                Debug.Log($"[Block] Wall Color at index {wallIndex}: {wallColor}");

                if (wallColor == blockColor)
                {
                    Debug.Log($"[Block] Color MATCH: Block ({blockColor}) == Wall ({wallColor}). Removing block.");
                    GameService.Instance.blockManager.RemoveBlock(this);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning($"[Block] Color MISMATCH: Block ({blockColor}) ≠ Wall ({wallColor}). Block stays.");
                    // Optional: Bounce or reset logic can be added here.
                }
            }
            else
            {
                Debug.Log($"[Block] {blockColor} block is inside grid at {exitCell}. No wall interaction.");
            }
        }

        public bool IsInsideGrid(Vector2Int pos)
        {
            return gridManager.IsInBounds(pos.x, pos.y);
        }

        private void OnDrawGizmosSelected()
        {
            if (gridManager == null) return;
            Gizmos.color = Color.magenta;

            foreach (var offset in shapeOffsets)
            {
                var pos = gridOrigin + offset;
                Gizmos.DrawWireCube(gridManager.GetWorldPosition(pos.x, pos.y), Vector3.one * 0.95f);
            }
        }
    }
}