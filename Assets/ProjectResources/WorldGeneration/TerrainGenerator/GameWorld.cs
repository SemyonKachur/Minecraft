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

        private Camera mainCamera = default;
       
        private void Awake()
        {
            mainCamera = Camera.main;

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

                    chunkData.Renderer = chunk;
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                bool isDestroying = Input.GetMouseButtonDown(0);
                
               Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

               if (Physics.Raycast(ray,  out var hitInfo))
               {
                   Vector3 blockCenter;
                   if (isDestroying)
                   {
                       blockCenter = hitInfo.point - hitInfo.normal * ChunkRenderer.BlockScale / 2;
                   }
                   else
                   {
                       blockCenter = hitInfo.point + hitInfo.normal * ChunkRenderer.BlockScale / 2;
                   }
                   
                   Vector3Int blockWorldPosition = Vector3Int.FloorToInt(blockCenter / ChunkRenderer.BlockScale);
                   Vector2Int chunkPosition = GetChunkContainingBlock(blockWorldPosition);
                   if (ChunkDatas.TryGetValue(chunkPosition, out ChunkData chunkData))
                   {
                       Vector3Int chunkOrigin = new Vector3Int(chunkPosition.x, 0, chunkPosition.y) *
                                                ChunkRenderer.ChunkWidht;
                       if (isDestroying)
                       {
                           chunkData.Renderer.DestroyBlock(blockWorldPosition - chunkOrigin);
                       }
                       else
                       {
                           chunkData.Renderer.SpawnBlock(blockWorldPosition - chunkOrigin);
                       }
                   }
               }
            }
        }

        public Vector2Int GetChunkContainingBlock(Vector3Int blockWorldPosition)
        {
            return new Vector2Int(blockWorldPosition.x / ChunkRenderer.ChunkWidht,
                blockWorldPosition.z / ChunkRenderer.ChunkWidht);
        }
    }
}
