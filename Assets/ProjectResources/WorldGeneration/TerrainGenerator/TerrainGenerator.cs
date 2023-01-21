using System.Collections;
using System.Collections.Generic;
using Minecraft.WorldGeneration.Chunk;
using UnityEngine;

namespace Minecraft.WorldGeneration
{
    /// <summary>
    /// Класс, создающий террейн на основе чанков.
    /// </summary>
    public static class TerrainGenerator
    {
        public static BlockType[,,] GenerateTerrain(int xOffset, int zOffset)
        {
            var result = new BlockType[ChunkRenderer.ChunkWidht, ChunkRenderer.ChunkHeight, ChunkRenderer.ChunkWidht];

            for (int x = 0; x < ChunkRenderer.ChunkWidht; x++)
            {
                for (int z = 0; z < ChunkRenderer.ChunkWidht; z++)
                {
                    float height = Mathf.PerlinNoise((x + xOffset) * 0.2f, (z + zOffset) * 0.2f) * 10 + 10;

                    for (int y = 0; y < height; y++)
                    {
                        result[x, y, z] = BlockType.Grass;
                    }
                }
            }

            return result;
        }
    }
}
