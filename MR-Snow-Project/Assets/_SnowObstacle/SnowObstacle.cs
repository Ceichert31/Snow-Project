using UnityEngine;

namespace OrthoSnowSplat
{
    public class SnowObstacle : MonoBehaviour
    {
        [SerializeField] private BoxCollider box;

        public BoxCollider Box => box;

        private void Reset()
        {
            box = GetComponentInChildren<BoxCollider>();
        }

        private void Awake()
        {
            if (box == null) box = GetComponentInChildren<BoxCollider>();
        }

        private void OnEnable()  => SnowObstacleRegistry.Register(this);
        private void OnDisable() => SnowObstacleRegistry.Deregister(this);
    }

}
