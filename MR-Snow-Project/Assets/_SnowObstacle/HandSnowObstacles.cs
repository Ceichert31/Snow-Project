using UnityEngine;

namespace OrthoSnowSplat
{
    public class HandSnowObstacles : MonoBehaviour
    {
        [SerializeField] private Transform[] joints;
        [SerializeField] private Vector3 boxSize = new Vector3(0.03f, 0.03f, 0.03f);
        [SerializeField] private Vector3 boxCenter = Vector3.zero;

        private void OnEnable()
        {
            if (joints == null) return;

            for (int i = 0; i < joints.Length; i++)
            {
                Transform joint = joints[i];
                if (joint == null) continue;

                string childName = "SnowObstacleVolume";
                if (joint.Find(childName) != null) continue;

                var go = new GameObject(childName);
                go.transform.SetParent(joint, worldPositionStays: false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;

                var box = go.AddComponent<BoxCollider>();
                box.isTrigger = true;
                box.center = boxCenter;
                box.size = boxSize;

                go.AddComponent<SnowObstacle>();
            }
        }
    }
}