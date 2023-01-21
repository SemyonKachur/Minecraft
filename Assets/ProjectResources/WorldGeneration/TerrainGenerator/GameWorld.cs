using System.Collections.Generic;
using Minecraft.WorldGeneration.Chunk;
using UnityEngine;

namespace Minecraft.WorldGeneration
{
    /// <summary>
    /// Контроллер генерации игровой локации.
    /// </summary>
    public class GameWorld : MonoBehaviour
    {
        public Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>();

        public ChunkRenderer ChunkPrefab = default;
       
        private void Awake()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    float xPos = x * ChunkRenderer.ChunkWidht * ChunkRenderer.BlockScale;
                    float zPos = y * ChunkRenderer.ChunkWidht * ChunkRenderer.BlockScale;
                    
                    ChunkData chunkData = new ChunkData();
                    chunkData.ChunkPosition = new Vector2Int(x, y);
                    chunkData.Blocks = TerrainGenerator.GenerateTerrain((int)xPos, (int)zPos);
                    ChunkDatas.Add(new Vector2Int(x,y), chunkData);

                    var chunk = Instantiate(ChunkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
                    chunk.ChunkData = chunkData;
                    chunk.ParentWorld = this;
                }
            }
        }
    }
}
