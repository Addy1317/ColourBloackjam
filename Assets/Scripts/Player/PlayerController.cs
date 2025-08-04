using SlowpokeStudio.Grid;
using UnityEngine;
using SlowpokeStudio.ColourBlocks;

namespace SlowpokeStudio.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] internal Camera mainCamera;
        [SerializeField] internal GridManager gridManager;

        private Block selectedBlock = null;
        private Vector2Int lastGridPosition;
        private bool isDragging = false;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("[PlayerController] Mouse Click Detected");
                TrySelectBlock();
            }

            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TrySelectBlock();
            }

            if (Input.GetMouseButton(0) && selectedBlock != null)
            {
                Vector3 worldPos = GetMouseWorldPosition();
                Vector2Int gridPos = gridManager.GetGridPosition(worldPos);

                if (gridPos != lastGridPosition && selectedBlock.CanMoveTo(gridPos))
                {
                    //Debug.Log($"[PlayerController] Moving block to: {gridPos}");
                    selectedBlock.MoveTo(gridPos);
                    lastGridPosition = gridPos;
                    selectedBlock.CheckWallExit();
                    isDragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectedBlock != null)
                {
                    selectedBlock.ReleaseBlock();
                    Debug.Log($"[PlayerController] Released block at {selectedBlock.gridOrigin}");
                    selectedBlock = null;
                    isDragging = false;
                }
            }
        }

        private void TrySelectBlock()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 2f); // Visual in Scene

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"[PlayerController] Ray hit: {hit.collider.name}");

                Block block = hit.collider.GetComponentInParent<Block>();
                if (block != null)
                {
                    selectedBlock = block;
                    lastGridPosition = block.gridOrigin;
                    selectedBlock.PrepareForMove();
                    Debug.Log($"[PlayerController] Selected block at {selectedBlock.gridOrigin}");
                }
                else
                {
                    Debug.Log("[PlayerController] Hit object has no Block component");
                }
            }
            else
            {
                Debug.Log("[PlayerController] No block hit");
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = Mathf.Abs(mainCamera.transform.position.z);
            return mainCamera.ScreenToWorldPoint(screenPoint);
        }

        private void OnDrawGizmos()
        {
            if (selectedBlock == null || gridManager == null)
                return;

            Gizmos.color = Color.cyan;

            foreach (var offset in selectedBlock.shapeOffsets)
            {
                Vector2Int cell = selectedBlock.gridOrigin + offset;
                Vector3 worldPos = gridManager.GetWorldPosition(cell);
                Gizmos.DrawWireCube(worldPos, Vector3.one * gridManager.cellSize * 0.95f);
            }

            Gizmos.color = Color.green;
            Vector3 originWorld = gridManager.GetWorldPosition(selectedBlock.gridOrigin);
            Gizmos.DrawSphere(originWorld, 0.1f);
        }
    }
}
