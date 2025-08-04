using SlowpokeStudio.ColourBlocks;
using SlowpokeStudio.levelData;
using SlowpokeStudio.Wall;
using UnityEngine;

namespace SlowpokeStudio
{
    public class GameManager : MonoBehaviour
    {
        [Header("Level Data")]
        [SerializeField] internal LevelsDatabaseSO levelsDatabase;
        [SerializeField] internal int currentLevelIndex = 0;

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
            LevelData levelData = levelsDatabase.levels[levelIndex];

            Debug.Log($"[GameManager] Loading Level {levelData.levelName} (Index {levelIndex})");

            // Clear previous blocks
            GameService.Instance.blockManager.ClearAllBlocks();

            // Clear activeBlocks list
            GameService.Instance.blockManager.activeBlocks.Clear();

            // Initialize wall segments for new level
            GameService.Instance.wallManager.InitializeWalls(levelData);

            // Spawn new blocks for the level
            GameService.Instance.blockManager.SpawnBlocksForLevel(levelData);
        }
        

        // Optional: Restart current level
        public void RestartLevel()
        {
            LoadLevel(currentLevelIndex);
        }

        public void LevelComplete()
        {
            Debug.Log($"[GameManager] Level {currentLevelIndex} COMPLETE!");

            GameService.Instance.uiManager.ShowLevelCompletePanel(currentLevelIndex + 1, currentLevelIndex + 2);
            // TODO: Show UI panel for level complete
            // TODO: Trigger next level on button press
        }
    }
}