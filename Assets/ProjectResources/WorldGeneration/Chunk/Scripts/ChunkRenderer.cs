using System.Collections.Generic;
using UnityEngine;

namespace Minecraft.WorldGeneration.Chunk
{
    /// <summary>
    /// Класс для генерации меша чанка.
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        public const int ChunkWidht = 10;
        public const int ChunkHeight = 128;
        public const float BlockScale = 0.5f;

        public ChunkData ChunkData = default;
        public GameWorld ParentWorld = default;

        [SerializeField] private Material chunkMaterial = default;
        
        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();

        private void Start()
        {
            Mesh chunkMesh = new Mesh();

            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkWidht; x++)
                {
                    for (int z = 0; z < ChunkWidht; z++)
                    {
                        GenerateBlock(x,y,z);
                    }
                }
            }

            chunkMesh.vertices = vertices.ToArray();
            chunkMesh.triangles = triangles.ToArray();

            chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();
            chunkMesh.Optimize();

            GetComponent<MeshFilter>().mesh = chunkMesh;
            GetComponent<MeshCollider>().sharedMesh = chunkMesh;
        }

        private void GenerateBlock(int x, int y, int z)
        {
            Vector3Int blockPosition = new Vector3Int(x,y,z);
            
            if(GetBlockAtPosition(blockPosition) == 0) return;

            if(GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) GenerateRightSide(blockPosition);
            if(GetBlockAtPosition(blockPosition + Vector3Int.left) == 0) GenerateLeftSide(blockPosition);
            if(GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0) GenerateFrontSide(blockPosition);
            if(GetBlockAtPosition(blockPosition + Vector3Int.back) == 0) GenerateBackSide(blockPosition);
            if(GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) GenerateTopSide(blockPosition);
            if(GetBlockAtPosition(blockPosition + Vector3Int.down) == 0)GenerateBottomSide(blockPosition);
        }

        private BlockType GetBlockAtPosition(Vector3Int blockPosition)
        {
            if (blockPosition.x >= 0 && blockPosition.x < ChunkWidht &&
                blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
                blockPosition.z >= 0 && blockPosition.z < ChunkWidht)
            {
                return ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
            }
            else
            {
                if ((blockPosition.y < 0) || blockPosition.y >= ChunkWidht) 
                    return BlockType.Air;
                
                Vector2Int adjacentChunkPosition = ChunkData.ChunkPosition;
                if (blockPosition.x < 0)
                {
                    adjacentChunkPosition.x--;
                    blockPosition.x += ChunkWidht;
                }
                else if (blockPosition.x >= ChunkWidht)
                {
                    adjacentChunkPosition.x++;
                    blockPosition.x -= ChunkWidht;
                }
                
                if (blockPosition.z < 0)
                {
                    adjacentChunkPosition.y--;
                    blockPosition.z += ChunkWidht;
                }
                else if (blockPosition.z >= ChunkWidht)
                {
                    adjacentChunkPosition.y++;
                    blockPosition.z -= ChunkWidht;
                }

                if(ParentWorld.ChunkDatas.TryGetValue(adjacentChunkPosition, out ChunkData adjacentChunk))
                {
                    return adjacentChunk.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
                }
                else
                {
                    return BlockType.Air;
                }
            }
        }

        private void GenerateRightSide(Vector3Int blockPosition)
        {
            vertices.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

            AddVerticesSquare();
        }
        
        private void GenerateLeftSide(Vector3Int blockPosition)
        {
            vertices.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);

            AddVerticesSquare();
        }
        
        private void GenerateFrontSide(Vector3Int blockPosition)
        {
            vertices.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

            AddVerticesSquare();
        }

        private void GenerateBackSide(Vector3Int blockPosition)
        {
            vertices.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);

            AddVerticesSquare();
        }
        
        private void GenerateTopSide(Vector3Int blockPosition)
        {
            vertices.Add((new Vector3(0, 1, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 1, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 1, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 1, 1) + blockPosition) * BlockScale);

            AddVerticesSquare();
        }
        
        private void GenerateBottomSide(Vector3Int blockPosition)
        {
            vertices.Add((new Vector3(0, 0, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 0, 0) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(0, 0, 1) + blockPosition) * BlockScale);
            vertices.Add((new Vector3(1, 0, 1) + blockPosition) * BlockScale);

            AddVerticesSquare();
        }
        private void AddVerticesSquare()
        {
            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);

            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 2);
        }
    }
}
