using UnityEngine;

namespace SlowpokeStudio.Save
{
    public static class SaveManager
    {
        private const string TotalGoldKey = "TotalGold";
        private const string CurrentLevelKey = "CurrentLevel";
        private const string HighestLevelKey = "HighestLevelReached";

        // ✅ Save Methods
        public static void SaveTotalGold(int gold)
        {
            PlayerPrefs.SetInt(TotalGoldKey, gold);
            Debug.Log($"[SaveManager] Total Gold Saved: {gold}");
        }

        public static void SaveCurrentLevel(int level)
        {
            PlayerPrefs.SetInt(CurrentLevelKey, level);
            Debug.Log($"[SaveManager] Current Level Saved: {level}");
        }

        public static void SaveHighestLevel(int level)
        {
            int highest = GetHighestLevel();
            if (level > highest)
            {
                PlayerPrefs.SetInt(HighestLevelKey, level);
                Debug.Log($"[SaveManager] New Highest Level Saved: {level}");
            }
        }

        public static void SaveAll(int gold, int currentLevel)
        {
            SaveTotalGold(gold);
            SaveCurrentLevel(currentLevel);
            SaveHighestLevel(currentLevel);
            PlayerPrefs.Save(); // Ensure write to disk
        }

        // ✅ Load Methods
        public static int GetTotalGold()
        {
            return PlayerPrefs.GetInt(TotalGoldKey, 0);
        }

        public static int GetCurrentLevel()
        {
            return PlayerPrefs.GetInt(CurrentLevelKey, 0);
        }

        public static int GetHighestLevel()
        {
            return PlayerPrefs.GetInt(HighestLevelKey, 0);
        }

        // ✅ Clear Save (for testing)
        public static void ResetAllData()
        {
            PlayerPrefs.DeleteKey(TotalGoldKey);
            PlayerPrefs.DeleteKey(CurrentLevelKey);
            PlayerPrefs.DeleteKey(HighestLevelKey);
            PlayerPrefs.Save();
            Debug.Log("[SaveManager] All saved data cleared.");
        }
    }
}
