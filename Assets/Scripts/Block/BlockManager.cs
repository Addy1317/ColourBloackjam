using SlowpokeStudio.Grid;
using SlowpokeStudio.levelData;
using UnityEngine;

namespace SlowpokeStudio.Block
{
    public enum BlockColor
    {
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

        private void SpawnBlock(BlockShapeData shapeData)
        {
            Vector2Int[] shapeOffsets = BlockShapeLibrary.GetShapeOffsets(shapeData.blockShapeType);

            GameObject blockGO = new GameObject($"Block_{shapeData.color}");
            blockGO.transform.SetParent(blockContainer);

            Block block = blockGO.AddComponent<Block>();

            block.Initialize(shapeData.color, shapeData.origin, shapeOffsets, gridManager);

            foreach (var offset in shapeOffsets)
            {
                Vector2Int cell = shapeData.origin + offset;
                Vector3 worldPos = gridManager.GetWorldPosition(cell.y, cell.x);

                GameObject tile = Instantiate(tilePrefab, worldPos, Quaternion.identity, blockGO.transform);
                tile.transform.localScale = Vector3.one * 0.9f;

                if (!tile.TryGetComponent<Collider>(out _))
                {
                    tile.AddComponent<BoxCollider>();
                }

                if (tile.TryGetComponent(out MeshRenderer mr))
                {
                    mr.material.color = GetColor(shapeData.color);
                }

                gridManager.SetCell(cell.x, cell.y, true);
                Debug.DrawRay(worldPos, Vector3.up, Color.cyan, 1f);
            }

            blockGO.transform.position = gridManager.GetWorldPosition(shapeData.origin.x, shapeData.origin.y);
        }

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

