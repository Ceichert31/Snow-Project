using UnityEngine;

public class PhysicsObjectSpawner : MonoBehaviour
{
    public int objectNum = 1000;
    public GameObject physicsObject;

    private void Start()
    {
        for (int i = 0; i < objectNum; i++)
        {
            Instantiate(physicsObject, transform.position, Quaternion.identity, transform);
        }
    }
}
