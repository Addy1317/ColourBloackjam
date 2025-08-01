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

