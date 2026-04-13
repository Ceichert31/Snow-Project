using System;
using UnityEngine;

/// <summary>
/// Listens for event to spawn a cube in front of the player
/// </summary>
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform cubeParent;

    //Maybe make object pooled in case people spam it?
    //Give cubes a liftime?

    //Cube needs rigidbody and own script for managing lifetime + VR scripts for raycast grab

    private void Awake()
    {
        cubeParent = new GameObject("CubeParent").transform;
    }

    /// <summary>
    /// Spawns a cube right in front of the player
    /// </summary>
    private void SpawnCube()
    {
        Instantiate(cubePrefab, transform.position, Quaternion.identity, cubeParent);
    }
}