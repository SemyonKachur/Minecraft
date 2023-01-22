using UnityEngine;

namespace Minecraft.WorldGeneration.Chunk
{
    /// <summary>
    /// Данные чанка.
    /// </summary>
    public class ChunkData : MonoBehaviour
    {
        public Vector2Int ChunkPosition = default;
        public ChunkRenderer Renderer = default;
        public BlockType[,,] Blocks = default;
    }
}
