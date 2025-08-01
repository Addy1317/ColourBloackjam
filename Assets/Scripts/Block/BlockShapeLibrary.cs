using UnityEngine;

namespace SlowpokeStudio.ColourBlocks
{
    public enum BlockShapeType
    {
        Single,
        LineH,
        LineV,
        LShape,
        Square,
        TShape
    }

    public class BlockShapeLibrary : MonoBehaviour
    {
        public static Vector2Int[] GetShapeOffsets(BlockShapeType shapeType)
        {
            switch (shapeType)
            {
                case BlockShapeType.Single:
                    return new Vector2Int[] { new Vector2Int(0, 0) };

                case BlockShapeType.LineH:
                    return new Vector2Int[]
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1)
                    };

                case BlockShapeType.LineV:
                    return new Vector2Int[]
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0)
                    };

                case BlockShapeType.LShape:
                    return new Vector2Int[]
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)
                    };

                case BlockShapeType.Square:
                    return new Vector2Int[]
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0),
                        new Vector2Int(1, 1)
                    };

                case BlockShapeType.TShape:
                    return new Vector2Int[]
                    {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, -1),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0)
                    };

                default:
                    Debug.LogError($"[BlockShapeLibrary] Shape type {shapeType} not implemented.");
                    return new Vector2Int[] { new Vector2Int(0, 0) };
            }
        }
    }
}

