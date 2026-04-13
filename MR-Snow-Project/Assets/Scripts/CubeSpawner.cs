using System;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Listens for event to spawn a cube in front of the player
/// </summary>
public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform cubeParent;

    private EventBinding<SpawnCubeEvent> spawnCubeBinding;

    //Maybe make object pooled in case people spam it?
    //Give cubes a liftime?

    //Cube needs rigidbody and own script for managing lifetime + VR scripts for raycast grab

    private void Awake()
    {
        cubeParent = new GameObject("CubeParent").transform;
    }

    private void OnEnable()
    {
        spawnCubeBinding = new(SpawnCube);
        EventBus<SpawnCubeEvent>.Register(spawnCubeBinding);
    }

    private void OnDisable()
    {
        EventBus<SpawnCubeEvent>.Deregister(spawnCubeBinding);
    }

    /// <summary>
    /// Spawns a cube right in front of the player
    /// </summary>
    [Button("Spawn Cube")]
    private void SpawnCube()
    {
        Instantiate(cubePrefab, transform.position, Quaternion.identity, cubeParent);
    }
}

public class SpawnCubeEvent : IEvent
{
}