using SlowpokeStudio.ColourBlocks;
using SlowpokeStudio.levelData;
using UnityEngine;

namespace SlowpokeStudio.Wall
{
    public class WallManager : MonoBehaviour
    {
        [System.Serializable]
        public struct WallSegment
        {
            public GameObject wallObject;
            public MeshRenderer meshRenderer;
        }

        [Header("Wall Segments in Clockwise Order (Index 0-7)")]
        public WallSegment[] wallSegments = new WallSegment[8]; // Assign in Inspector

        public void InitializeWalls(LevelData levelData)
        {
            if(levelData.wallSegments.Length != wallSegments.Length)
            {
                Debug.LogError($"[WallManager] Wall count mismatch: LevelData has {levelData.wallSegments.Length}, WallManager has {wallSegments.Length}.");
                return;
            }

            for (int i = 0; i < wallSegments.Length; i++)
            {
                WallSegmentInfo segmentInfo = levelData.wallSegments[i];
                WallSegment wallSegment = wallSegments[i];

                if (wallSegment.wallObject == null || wallSegment.meshRenderer == null)
                {
                    Debug.LogWarning($"[WallManager] Wall {i} GameObject or MeshRenderer is missing.");
                    continue;
                }

                wallSegment.wallObject.SetActive(true); // Always ensure wall is active

                Color wallColor = GetUnityColor(segmentInfo.color);
                wallSegment.meshRenderer.material.color = wallColor;

                Debug.Log($"[WallManager] Wall {i} color set to {segmentInfo.color} ({wallColor}).");
            }
        }

        private Color GetUnityColor(BlockColor color)
        {
            return color switch
            {
                BlockColor.Red => Color.red,
                BlockColor.Blue => Color.blue,
                BlockColor.Green => Color.green,
                BlockColor.Yellow => Color.yellow,
                _ => Color.white
            };
        }
    }
}
