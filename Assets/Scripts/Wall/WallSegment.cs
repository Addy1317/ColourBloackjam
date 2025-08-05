using SlowpokeStudio.ColourBlocks;
using UnityEngine;
using SlowpokeStudio.Audio;

namespace SlowpokeStudio.Wall
{
    public class WallSegment : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError("[WallSegment] MeshRenderer missing.");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Block block = other.GetComponentInParent<Block>();
            if (block == null)
            {
                Debug.Log("[WallSegment] Non-block entered. Ignored.");
                return;
            }

            Color wallColor = meshRenderer.material.color;
            Color blockColor = GetUnityColor(block.blockColor);

            Debug.Log($"[WallSegment] Detected Block {block.blockColor} with color ({blockColor}) at Wall ({wallColor})");

            if (ApproximatelyEqualColors(wallColor, blockColor))
            {
                GameService.Instance.audioManager.PlaySFX(SFXType.OnBlockPlacedOnWallSFX);
                Debug.Log("[WallSegment] Color MATCH. Removing Block.");
                GameService.Instance.blockManager.RemoveBlock(block);
                Destroy(block.gameObject);
            }
            else
            {
                GameService.Instance.audioManager.PlaySFX(SFXType.OnBlockPlacedOnWrongWallSFX);
                Debug.LogWarning("[WallSegment] Color MISMATCH. Block stays.");
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

        private bool ApproximatelyEqualColors(Color a, Color b)
        {
            return Mathf.Approximately(a.r, b.r) &&
                   Mathf.Approximately(a.g, b.g) &&
                   Mathf.Approximately(a.b, b.b);
        }
    }
}
