using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.ARFoundation;

namespace ARExtensions
{
    /// <summary>
    /// Listens for changes in current AR planes and subdivides new planes
    /// </summary>
    [RequireComponent(typeof(ARPlaneManager))]
    public class SubdivideARPlanes : MonoBehaviour
    {
        [Header("Subdivision Settings")] [SerializeField] [Range(1, 5)]
        private int subdivisionCount = 2;

        /// <remarks>
        /// Invoked by <see cref="ARPlaneManager"/>
        /// </remarks>
        public void OnPlanesModified(ARTrackablesChangedEventArgs<ARPlane> ctx)
        {
            foreach (var plane in ctx.added)
            {
                UpdatePlaneMesh(plane);
            }

            //Maybe switch to async if performance isn't great?
            //Will have to check if Quest can handle parallel processes
            //Parallel.ForEach(ctx.added, UpdatePlaneMesh);
            foreach (var plane in ctx.updated)
            {
                UpdatePlaneMesh(plane);
            }
        }

        /// <summary>
        /// Updates newly added or modified planes with a new subdivided mesh
        /// </summary>
        /// <param name="plane"></param>
        private void UpdatePlaneMesh(ARPlane plane)
        {
            var meshFilter = plane.GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            var size = plane.size;

            meshFilter.mesh = CreateSubdividedMesh((int)size.x, (int)size.y, subdivisionCount);
        }

        /// <summary>
        /// Creates a subdivied mesh and returns in
        /// </summary>
        /// <returns></returns>
        private Mesh CreateSubdividedMesh(int sizeX, int sizeY, int subdivisions)
        {
            Mesh mesh = new Mesh();

            mesh.name = $"Subdivided Mesh ({subdivisionCount})";

            //Initialize an array big enough to hold all vertices data
            Vector3[] verts = new Vector3[((sizeX + 1) * (sizeY + 1))];
            Vector2[] uvs = new Vector2[verts.Length];

            //Create new vertices
            for (int x = 0, i = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++, i++)
                {
                    verts[i] = new Vector3(x, 0, y);
                    uvs[i] = new Vector2((float)x / sizeX, (float)y / sizeY);
                }
            }


            int[] tris = new int[sizeX * 6];

            //Calculate triangles
            for (int i = 0, j = 0, x = 0; x < sizeX; x++, i += 6, j++)
            {
                for (int y = 0; y < sizeY; y++, i += 6, j++)
                {
                    tris[i] = j;
                    tris[j + 3] = tris[i + 2] = j + 1;
                    tris[i + 4] = tris[i + 1] = j + sizeX + 1;
                    tris[i + 5] = j + sizeX + 2;
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