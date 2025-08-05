using SlowpokeStudio.Currency;
using SlowpokeStudio.levelData;
using UnityEngine;

namespace SlowpokeStudio
{
    public class GameManager : MonoBehaviour
    {
        [Header("Level Data")]
        [SerializeField] internal LevelsDatabaseSO levelsDatabase;
        [SerializeField] internal int currentLevelIndex = 0;

        private LevelData currentLevelData;
        private bool levelCompleteTriggered = false;

        private void Start()
        {
            LoadLevel(currentLevelIndex);
            GameService.Instance.wallManager.InitializeWalls(levelsDatabase.levels[currentLevelIndex]);
        }

        public void LoadLevel(int levelIndex)
        {
            levelCompleteTriggered = false;

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
            GameService.Instance.timerManager.StartTimer(levelData.timeLimit); // Set in seconds

            Debug.Log($"[GameManager] Loading Level {levelData.levelName} (Index {levelIndex})");

            // Clear previous blocks
            GameService.Instance.blockManager.ClearAllBlocks();

            // Clear activeBlocks list
            GameService.Instance.blockManager.activeBlocks.Clear();

            // Initialize wall segments for new level
            GameService.Instance.wallManager.InitializeWalls(levelData);

            // Spawn new blocks for the level
            GameService.Instance.blockManager.SpawnBlocksForLevel(levelData);

            GameService.Instance.uiManager.UpdateLevelText(levelIndex + 1);
        }
        
        public void LevelComplete()
        {
            if (levelCompleteTriggered)
            {
                Debug.LogWarning("[GameManager] LevelComplete called again. Ignored.");
                return;
            }
            levelCompleteTriggered = true;

            LevelData levelData = levelsDatabase.levels[currentLevelIndex];
            int reward = levelData.goldReward;

            Debug.Log($"[GameManager] LevelComplete Triggered. Reward: {reward}");

            GameService.Instance.currencyManager.AddGold(reward);

            Debug.Log($"[GameManager] Gold Added. Current Total (from CurrencyManager): {GameService.Instance.currencyManager.TotalGoldDebug()}");

            Debug.Log($"[GameManager] Level {currentLevelIndex} COMPLETE!");

            GameService.Instance.uiManager.ShowLevelCompletePanel(currentLevelIndex + 1, currentLevelIndex + 2);
        }
    }
}