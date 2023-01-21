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
        private const int ChunkWidth = 10;
        private const int ChunkHeight = 128;

        public int[,,] Blocks = new int[ChunkWidth, ChunkHeight, ChunkWidth];

        [SerializeField] private Material chunkMaterial = default;
        
        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles = new List<int>();

        private void Start()
        {
            Mesh chunkMesh = new Mesh();

            Blocks[0, 0, 0] = 2;
            
            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int x = 0; x < ChunkWidth; x++)
                {
                    for (int z = 0; z < ChunkWidth; z++)
                    {
                        GenerateBlock(x,y,z);
                    }
                } 
            }
            
            chunkMesh.vertices = vertices.ToArray();
            chunkMesh.triangles = triangles.ToArray();

            chunkMesh.RecalculateBounds();
            chunkMesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = chunkMesh;
            GetComponent<MeshRenderer>().material = chunkMaterial;
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

        private int GetBlockAtPosition(Vector3Int blockPosition)
        {
            if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
                blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
                blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
            {
                return Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
            }
            else
            {
                return 0;
            }
        }

        private void GenerateRightSide(Vector3Int blockPosition)
        {
            vertices.Add(new Vector3(0, 0, 0) + blockPosition);
            vertices.Add(new Vector3(0, 0, 1) + blockPosition);
            vertices.Add(new Vector3(0, 1, 0) + blockPosition);
            vertices.Add(new Vector3(0, 1, 1) + blockPosition);

            AddVerticesSquare();
        }
        
        private void GenerateLeftSide(Vector3Int blockPosition)
        {
            vertices.Add(new Vector3(1, 0, 0) + blockPosition);
            vertices.Add(new Vector3(1, 1, 0) + blockPosition);
            vertices.Add(new Vector3(1, 0, 1) + blockPosition);
            vertices.Add(new Vector3(1, 1, 1) + blockPosition);

            AddVerticesSquare();
        }
        
        private void GenerateFrontSide(Vector3Int blockPosition)
        {
            vertices.Add(new Vector3(0, 0, 0) + blockPosition);
            vertices.Add(new Vector3(0, 1, 0) + blockPosition);
            vertices.Add(new Vector3(1, 0, 0) + blockPosition);
            vertices.Add(new Vector3(1, 1, 0) + blockPosition);

            AddVerticesSquare();
        }

        private void GenerateBackSide(Vector3Int blockPosition)
        {
            vertices.Add(new Vector3(0, 0, 1) + blockPosition);
            vertices.Add(new Vector3(1, 0, 1) + blockPosition);
            vertices.Add(new Vector3(0, 1, 1) + blockPosition);
            vertices.Add(new Vector3(1, 1, 1) + blockPosition);

            AddVerticesSquare();
        }
        
        private void GenerateTopSide(Vector3Int blockPosition)
        {
            vertices.Add(new Vector3(0, 1, 0) + blockPosition);
            vertices.Add(new Vector3(0, 1, 1) + blockPosition);
            vertices.Add(new Vector3(1, 1, 0) + blockPosition);
            vertices.Add(new Vector3(1, 1, 1) + blockPosition);

            AddVerticesSquare();
        }
        
        private void GenerateBottomSide(Vector3Int blockPosition)
        {
            vertices.Add(new Vector3(0, 0, 0) + blockPosition);
            vertices.Add(new Vector3(1, 0, 0) + blockPosition);
            vertices.Add(new Vector3(0, 0, 1) + blockPosition);
            vertices.Add(new Vector3(1, 0, 1) + blockPosition);

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
