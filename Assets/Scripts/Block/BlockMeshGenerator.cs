using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Block
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BlockMeshGenerator : MonoBehaviour
    {
        public float tileSize = 1f;

        public void GenerateMesh(Vector2Int[] shapeOffsets)
        {
            Debug.Log($"[MeshGen] Generated shape with {shapeOffsets.Length} tiles for {gameObject.name}");
            if (shapeOffsets == null || shapeOffsets.Length == 0)
            {
                Debug.LogWarning("[BlockMeshGenerator] Empty shapeOffsets array.");
                return;
            }

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            mesh.name = "BlockShapeMesh";

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            for (int i = 0; i < shapeOffsets.Length; i++)
            {
                Vector2Int offset = shapeOffsets[i];
                int vertexIndex = vertices.Count;

                Vector3 basePos = new Vector3(offset.x * tileSize, 0, offset.y * tileSize);

                vertices.Add(basePos);
                vertices.Add(basePos + new Vector3(tileSize, 0, 0));
                vertices.Add(basePos + new Vector3(tileSize, 0, tileSize));
                vertices.Add(basePos + new Vector3(0, 0, tileSize));

                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 3);

                uvs.AddRange(new Vector2[]
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(1, 1),
                    new Vector2(0, 1)
                });
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetUVs(0, uvs);
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;

            Debug.Log($"[BlockMeshGenerator] Mesh generated with {shapeOffsets.Length} tiles.");
        }
    }
}

