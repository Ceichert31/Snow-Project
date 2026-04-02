using System;
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
        [Header("Subdivision Settings")] [SerializeField]
        [Range(1, 5)]
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
            
        }

        /// <summary>
        /// Creates a subdivied mesh and returns in
        /// </summary>
        /// <returns></returns>
        private Mesh CreateSubdividedMesh()
        {
            return null;
        }
    }
}
