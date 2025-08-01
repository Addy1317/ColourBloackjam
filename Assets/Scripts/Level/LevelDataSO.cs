#if UNITY_EDITOR
using UnityEditor;
#endif
using SlowpokeStudio.ColourBlocks;
using UnityEngine;

namespace SlowpokeStudio.levelData
{
    public enum LevelGoalType
    {
        ClearAllBlocks,
        TimeLimit,
        MoveLimit,
        Custom // For future expansion
    }

    [System.Serializable]
    public struct BlockInfo
    {
        public Vector2Int gridPosition;
        public BlockColor color;
    }

    [System.Serializable]
    public struct BlockShapeData
    {
        public Vector2Int origin;             // Pivot point in grid
        public BlockColor color;
        public BlockShapeType blockShapeType;     // Relative shape positions

#if UNITY_EDITOR
        public void ValidateGridBounds(int maxX, int maxY)
        {
            origin.x = Mathf.Clamp(origin.x, 0, maxX - 1);
            origin.y = Mathf.Clamp(origin.y, 0, maxY - 1);
        }
#endif
    }


    [System.Serializable]
    public struct WallSegmentInfo
    {
        public int index; // 0 to 7 (clockwise from top-left)
        public BlockColor color; // If wall is white/inactive, use a special value or flag
        public bool isActive;
    }

    [System.Serializable]
    public class LevelData
    {
        public string levelName = "Level 1";

        [Header("Block Layout")]
        public BlockShapeData[] blocks;

        [Header("Wall Segments")]
        public WallSegmentInfo[] wallSegments = new WallSegmentInfo[8];


        [Header("Completion")]
        public LevelGoalType goalType;
        public float timeLimit; // Only used if goalType == TimeLimit
        public int moveLimit;   // Only used if goalType == MoveLimit
    }

    [CreateAssetMenu(fileName = "LevelsDatabase", menuName = "ColorBlockJam/Levels Database")]
    public class LevelsDatabaseSO : ScriptableObject
    {
        public LevelData[] levels;
    }
}