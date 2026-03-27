using UnityEngine;

public class SnowDisplacementPlane : MonoBehaviour
{
    [SerializeField] private RenderTexture splatMap;

    private Material planeMaterial;

    private static readonly int SplatMapID = Shader.PropertyToID("_SplatMap");
    private static readonly int GroundOriginID = Shader.PropertyToID("_GroundOrigin");
    private static readonly int GroundSizeID = Shader.PropertyToID("_GroundSize");

    private void Start()
    {
        planeMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (splatMap == null || planeMaterial == null)
            return;

        Vector3 scale = transform.lossyScale;

        planeMaterial.SetTexture(SplatMapID, splatMap);
        planeMaterial.SetVector(GroundOriginID, new Vector2(transform.position.x, transform.position.z));
        planeMaterial.SetVector(GroundSizeID, new Vector2(scale.x, scale.z));
    }
}