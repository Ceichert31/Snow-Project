using System;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;

namespace ARExtensions
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class TestSubdivision : MonoBehaviour
    {
        [Tooltip("How many subdivisions will be performed")]
        [Header("Subdivision Settings")]
        [SerializeField]
        [UnityEngine.Range(1, 100)]
        private int subdivisionCount = 2;

        [Tooltip("The size of the plane")] [SerializeField]
        private Vector2Int size;

        [HorizontalLine] [Header("Subdivision References")] [SerializeField]
        private MeshFilter meshFilter;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        /// <summary>
        /// Updates newly added or modified planes with a new subdivided mesh
        /// </summary>
        [Button("Subdivide Mesh")]
        private void UpdatePlaneMesh()
        {
            Assert.IsNotNull(meshFilter);

            meshFilter.mesh = CreateSubdividedMesh(size.x, size.y, subdivisionCount);
        }

        /// <summary>
        /// Creates a subdivided mesh and returns in
        /// </summary>
        /// <returns></returns>
        private Mesh CreateSubdividedMesh(int sizeX, int sizeY, int subdivisions)
        {
            Mesh mesh = new Mesh();

            mesh.name = $"Subdivided Mesh ({subdivisionCount})";

            int vertsPerSide = subdivisionCount + 1;

            //Initialize an array big enough to hold all vertices data
            Vector3[] verts = new Vector3[vertsPerSide * vertsPerSide];
            Vector2[] uvs = new Vector2[verts.Length];

            //Need to swap from generating 4 verts based on sizex sizey to generating subdivisions


            //Create new vertices
            for (int x = 0, i = 0; x < vertsPerSide; x++)
            {
                for (int y = 0; y < sizeY; y++, i++)
                {
                    float u = (float)x / subdivisions;
                    float v = (float)y / subdivisions;

                    verts[i] = new Vector3((u - 0.5f) * sizeX, 0, (v - 0.5f) * sizeY);
                    uvs[i] = new Vector2(u, v);
                }
            }

            int[] tris = new int[subdivisions * subdivisions * 6];

            /*//Calculate triangles
        for (int i = 0, j = 0, x = 0; x < sizeX; x++, i += 6, j++)
        {
            for (int y = 0; y < sizeY; y++, i += 6, j++)
            {
                tris[i] = j;
                tris[j + 3] = tris[i + 2] = j + 1;
                tris[i + 4] = tris[i + 1] = j + sizeX + 1;
                tris[i + 5] = j + sizeX + 2;
            }
        }*/

            for (int x = 0, t = 0; x < subdivisions; x++)
            {
                for (int y = 0; y < subdivisions; y++)
                {
                    //Calculate corners of quad
                    int bottomLeft = x * vertsPerSide + y;
                    int bottomRight = bottomLeft + 1;
                    int topLeft = bottomLeft + vertsPerSide;
                    int topRight = topLeft + 1;

                    //First triangle
                    tris[t++] = bottomLeft;
                    tris[t++] = topLeft;
                    tris[t++] = bottomRight;

                    //Second triangle
                    tris[t++] = bottomRight;
                    tris[t++] = topLeft;
                    tris[t++] = topRight;
                }
            }

            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}