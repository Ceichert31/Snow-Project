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
        var render = GetComponent<Renderer>();

        _ = render.sharedMaterial;

        planeMaterial = render.material;
    }

    private void Update()
    {
        if (splatMap == null || planeMaterial == null)
            return;

        if (!splatMap.IsCreated())
        {
            Debug.LogWarning("SplatMap Render Texture is missing!");
            return;
        }

        Vector3 scale = transform.lossyScale;

        planeMaterial.SetTexture(SplatMapID, splatMap);
        planeMaterial.SetVector(GroundOriginID, new Vector2(transform.position.x, transform.position.z));
        planeMaterial.SetVector(GroundSizeID, new Vector2(scale.x, scale.z));
    }
}