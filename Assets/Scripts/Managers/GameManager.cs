using SlowpokeStudio.levelData;
using UnityEngine;
using SlowpokeStudio.Save;
using SlowpokeStudio.levelDataInfo;

namespace SlowpokeStudio
{
    public class GameManager : MonoBehaviour
    {
        [Header("Level Data")]
        [SerializeField] internal LevelDatabaseSO levelDatabaseSO;
        [SerializeField] internal int currentLevelIndex = 0;
        [SerializeField] public int ActualGameLevelIndex => currentLevelIndex + 1;

        private LevelData currentLevelData;
        private bool levelCompleteTriggered = false;

        private void Awake()
        {
            if (levelDatabaseSO == null)
            {
                levelDatabaseSO = Resources.Load<LevelDatabaseSO>("LevelData/LevelDatabaseSO");
                Debug.Log("<color=cyan>[GameManager]</color> Loaded LevelsDatabaseSO from Resources.");
            }

            currentLevelIndex = SaveManager.GetCurrentLevel();
            Debug.Log("<color=cyan>[GameManager]</color> Loaded Current Level: " + currentLevelIndex);
        }

        private void Start()
        {
            LoadLevel(currentLevelIndex);
            //GameService.Instance.wallManager.InitializeWalls(levelsDatabase.levels[currentLevelIndex]);
        }

        public void LoadLevel(int levelIndex)
        {
            levelCompleteTriggered = false;

            if (levelDatabaseSO == null || levelDatabaseSO.levels.Length == 0)
            {
                Debug.LogError("<color=red>[GameManager]</color> LevelsDatabase is empty or missing."); return;
            }

            if (levelIndex < 0 || levelIndex >= levelDatabaseSO.levels.Length)
            {
                Debug.LogError("<color=red>[GameManager]</color> Invalid level index " + levelIndex); return;
            }

            currentLevelIndex = levelIndex;
            SaveManager.SaveCurrentLevel(currentLevelIndex);

            LevelData levelData = levelDatabaseSO.levels[levelIndex];

            GameService.Instance.blockManager.levelDatabaseSO = levelDatabaseSO;

            GameService.Instance.timerManager.StartTimer(levelData.timeLimit);

            Debug.Log("<color=cyan>[GameManager]</color> Loading Level " + levelData.levelName + " (Index " + levelIndex + ")");

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
                Debug.LogWarning("<color=yellow>[GameManager]</color> LevelComplete called again. Ignored."); return;
            }
            levelCompleteTriggered = true;

            LevelData levelData = levelDatabaseSO.levels[currentLevelIndex];
            int reward = levelData.goldReward;

            Debug.Log("<color=green>[GameManager]</color> LevelComplete Triggered. Reward: " + reward);

            GameService.Instance.currencyManager.AddGold(reward);
            SaveManager.SaveHighestLevel(currentLevelIndex + 1);

            Debug.Log("<color=green>[GameManager]</color> Gold Added. Current Total (from CurrencyManager): " + GameService.Instance.currencyManager.TotalGoldDebug());
            Debug.Log("<color=green>[GameManager]</color> Level " + currentLevelIndex + " COMPLETE!");

            GameService.Instance.uiManager.ShowLevelCompletePanel(currentLevelIndex + 1, currentLevelIndex + 2);
        }
    }
}