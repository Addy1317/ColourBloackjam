#if UNITY_EDITOR
using UnityEditor;
#endif
using SlowpokeStudio.ColourBlocks;
using UnityEngine;

namespace SlowpokeStudio.levelDataInfo
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
        public Vector2Int origin;
        public BlockColor color;
        public BlockShapeType blockShapeType;

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
        public int index;
        public BlockColor color;
        public bool isActive;
    }

    [System.Serializable]
    public class LevelData
    {
        public string levelName = "Level 1";

        [Header("Block Layout")]
        public BlockShapeData[] blocks;

        [Header("Wall Segments")]
        public WallSegmentInfo[] wallSegments;

        [Header("Completion")]
        public LevelGoalType goalType;
        public float timeLimit;

        [Header("Rewards")]
        public int goldReward;
    }
}
