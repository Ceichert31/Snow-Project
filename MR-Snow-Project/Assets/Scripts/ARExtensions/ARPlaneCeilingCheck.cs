using System;
using UnityEngine;

namespace ARExtensions
{
    /// <summary>
    /// Checks for the orientation of the AR plane, disables upside down planes
    /// </summary>
    public class ARPlaneCeilingCheck : MonoBehaviour
    {
        private void Start()
        {
            if (Vector3.Dot(Vector3.up, transform.up) <= -0.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}