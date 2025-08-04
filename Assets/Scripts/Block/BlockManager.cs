using SlowpokeStudio.Grid;
using SlowpokeStudio.levelData;
using UnityEngine;

namespace SlowpokeStudio.ColourBlocks
{
    public enum BlockColor
    {
        None,
        Red,
        Blue,
        Green,
        Yellow
    }

    public class BlockManager : MonoBehaviour
    {
        [Header("References")]
        public GridManager gridManager;
        public GameObject tilePrefab;
        public LevelsDatabaseSO levelsDatabase;
        public int currentLevelIndex = 0;

        [Header("Block Parent")]
        public Transform blockContainer;
        public Material defaultBlockMaterial;

        private void Start()
        {
            SpawnBlocksForLevel();
        }

        public void SpawnBlocksForLevel()
        {
            if (levelsDatabase == null || levelsDatabase.levels.Length == 0)
            {
                Debug.LogError("[BlockManager] No levels found in LevelsDatabase.");
                return;
            }

            LevelData levelData = levelsDatabase.levels[currentLevelIndex];

            foreach (var shapeData in levelData.blocks)
            {
                SpawnBlock(shapeData);
            }
        }

        #region Block Spawning Methods

        private void SpawnBlock(BlockShapeData shapeData)
        {
            GameObject blockGO = CreateBlockParent(shapeData);
            Block block = InitializeBlockData(blockGO, shapeData);

            foreach (var offset in block.shapeOffsets)
            {
                SpawnBlockTile(blockGO, shapeData.origin, offset, block.blockColor);
                SetGridOccupied(shapeData.origin + offset, true);
            }

            Debug.Log($"[BlockManager] Spawned {shapeData.blockShapeType} block at {shapeData.origin} with {block.shapeOffsets.Length} tiles.");
        }

        private GameObject CreateBlockParent(BlockShapeData shapeData)
        {
            GameObject blockGO = new GameObject($"Block_{shapeData.color}");
            blockGO.transform.SetParent(blockContainer);
            blockGO.transform.position = gridManager.GetWorldPosition(shapeData.origin);
            return blockGO;
        }

        private Block InitializeBlockData(GameObject blockGO, BlockShapeData shapeData)
        {
            Vector2Int[] shapeOffsets = BlockShapeLibrary.GetShapeOffsets(shapeData.blockShapeType);

            Block block = blockGO.AddComponent<Block>();
            block.Initialize(shapeData.color, shapeData.origin, shapeOffsets, gridManager);

            return block;
        }

        private void SpawnBlockTile(GameObject parent, Vector2Int origin, Vector2Int offset, BlockColor color)
        {
            Vector2Int cell = origin + offset;
            Vector3 worldPos = gridManager.GetWorldPosition(cell);
            Vector3 localOffset = worldPos - parent.transform.position;

            GameObject tile = Instantiate(tilePrefab, parent.transform);
            tile.transform.localPosition = localOffset;
            tile.transform.localScale = Vector3.one * 0.95f;

            if (!tile.TryGetComponent<Collider>(out _))
                tile.AddComponent<BoxCollider>();

            if (tile.TryGetComponent(out MeshRenderer mr))
                mr.material.color = GetColor(color);

            Debug.DrawRay(worldPos, Vector3.forward * 0.25f, Color.green, 2f);
        }

        private void SetGridOccupied(Vector2Int cell, bool occupied)
        {
            gridManager.SetCell(cell.x, cell.y, occupied);
        }

        #endregion
        private Color GetColor(BlockColor color)
        {
            return color switch
            {
                BlockColor.Red => Color.red,
                BlockColor.Blue => Color.blue,
                BlockColor.Green => Color.green,
                BlockColor.Yellow => Color.yellow,
                _ => Color.white,
            };
        }

        public void ClearAllBlocks()
        {
            foreach (Transform child in blockContainer)
            {
                Destroy(child.gameObject);
            }

            Debug.Log("[BlockManager] All blocks cleared from scene.");
        }
    }
}

