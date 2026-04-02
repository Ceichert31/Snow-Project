using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARExtensions
{
    /// <summary>
    /// Listens for changes in current AR planes and subdivides new planes
    /// </summary>
    [RequireComponent(typeof(ARPlaneManager))]
    public class SubdivideARPlanes : MonoBehaviour
    {
        [Header("Subdivision Settings")] [SerializeField] [Range(1, 100)]
        private int subdivisionCount = 2;

        private HashSet<TrackableId> _planesBeingUpdated = new();

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
                if (_planesBeingUpdated.Contains(plane.trackableId)) continue;

                _planesBeingUpdated.Add(plane.trackableId);
                UpdatePlaneMesh(plane);
                _planesBeingUpdated.Remove(plane.trackableId);
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
                for (int y = 0; y < vertsPerSide; y++, i++)
                {
                    float u = (float)x / subdivisions;
                    float v = (float)y / subdivisions;

                    verts[i] = new Vector3((u - 0.5f) * sizeX, 0, (v - 0.5f) * sizeY);
                    uvs[i] = new Vector2(u, v);
                }
            }

            int[] tris = new int[subdivisions * subdivisions * 6];

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
                    tris[t++] = bottomRight;
                    tris[t++] = topLeft;

                    //Second triangle
                    tris[t++] = bottomRight;
                    tris[t++] = topRight;
                    tris[t++] = topLeft;
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