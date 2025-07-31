using SlowpokeStudio.Grid;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlowpokeStudio.Block
{
    public class Block : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        [Header("Injected Data")]
        public BlockColor blockColor;
        public Vector2Int gridOrigin;
        public Vector2Int[] shapeOffsets;

        private GridManager gridManager;
        private bool isDragging;
        private Vector3 offset;

        public void Initialize(BlockColor color, Vector2Int origin, Vector2Int[] offsets, GridManager gridRef)
        {
            blockColor = color;
            gridOrigin = origin;
            shapeOffsets = offsets;
            gridManager = gridRef;

            Debug.Log($"[Block] Initialized at {gridOrigin} with shape {offsets.Length} tiles.");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition(eventData);

            foreach (var offset in shapeOffsets)
            {
                var pos = gridOrigin + offset;
                gridManager.SetCell(pos.x, pos.y, false);
                Debug.DrawRay(gridManager.GetWorldPosition(pos.x, pos.y), Vector3.up, Color.yellow, 0.5f);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            Vector3 mouseWorldPos = GetMouseWorldPosition(eventData) + offset;
            Vector2Int targetOrigin = gridManager.GetGridPosition(mouseWorldPos);

            if (targetOrigin != gridOrigin && CanMoveTo(targetOrigin))
            {
                gridOrigin = targetOrigin;
                transform.position = gridManager.GetWorldPosition(gridOrigin.x, gridOrigin.y);
                Debug.Log($"[Block] Moved to new gridOrigin: {gridOrigin}");
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            foreach (var offset in shapeOffsets)
            {
                var pos = gridOrigin + offset;
                gridManager.SetCell(pos.x, pos.y, true);
                Debug.DrawRay(gridManager.GetWorldPosition(pos.x, pos.y), Vector3.up * 0.5f, Color.green, 1f);
            }
        }

        private Vector3 GetMouseWorldPosition(PointerEventData eventData)
        {
            Vector3 screenPoint = eventData.position;
            screenPoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(screenPoint);
        }

        private bool CanMoveTo(Vector2Int newOrigin)
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

        private void OnMouseDown()
        {
            Debug.Log($"[Block] {gameObject.name} clicked.");
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

