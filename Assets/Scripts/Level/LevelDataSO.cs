using UnityEngine;
using SlowpokeStudio.levelDataInfo;

namespace SlowpokeStudio.levelData
{
    [CreateAssetMenu(fileName = "LevelDatabaseSO", menuName = "CodeBlockJam/Level Database")]
    public class LevelDatabaseSO : ScriptableObject
    {
        public LevelData[] levels;
    }
}