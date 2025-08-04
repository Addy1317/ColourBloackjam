using SlowpokeStudio.ColourBlocks;
using SlowpokeStudio.levelData;
using SlowpokeStudio.Wall;
using UnityEngine;

namespace SlowpokeStudio
{
    public class GameManager : MonoBehaviour
    {
        [Header("Level Data")]
        public LevelsDatabaseSO levelsDatabase;
        public int currentLevelIndex = 0;

        [Header("Managers")]
        //public WallManager wallManager;
       // public BlockManager blockManager;

        private LevelData currentLevelData;

        private void Start()
        {
            LoadLevel(currentLevelIndex);
            GameService.Instance.wallManager.InitializeWalls(levelsDatabase.levels[currentLevelIndex]);
        }

        public void LoadLevel(int levelIndex)
        {
            if (levelsDatabase == null || levelsDatabase.levels.Length == 0)
            {
                Debug.LogError("[GameManager] LevelsDatabase is empty or missing.");
                return;
            }

            if (levelIndex < 0 || levelIndex >= levelsDatabase.levels.Length)
            {
                Debug.LogError($"[GameManager] Invalid level index {levelIndex}.");
                return;
            }

            currentLevelIndex = levelIndex;
            currentLevelData = levelsDatabase.levels[levelIndex];

            Debug.Log($"[GameManager] Loading Level: {currentLevelData.levelName}");

            // Clear previous blocks
            GameService.Instance.blockManager.ClearAllBlocks();

            // Initialize wall segments
            GameService.Instance.wallManager.InitializeWalls(currentLevelData);

            // Spawn blocks for this level
            //blockManager.SpawnBlocksForLevel(currentLevelData);

            // Future: Initialize goal, timer, move count, etc.
        }

        // Optional: Restart current level
        public void RestartLevel()
        {
            LoadLevel(currentLevelIndex);
        }
    }
}