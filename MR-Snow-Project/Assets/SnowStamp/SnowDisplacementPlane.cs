using UnityEngine;

public class SnowDisplacementPlane : MonoBehaviour
{
    [SerializeField] private RenderTexture splatMap;

    private Renderer planeRenderer;
    private MaterialPropertyBlock propertyBlock;

    private static readonly int SplatMapID = Shader.PropertyToID("_SplatMap");
    private static readonly int GroundOriginID = Shader.PropertyToID("_GroundOrigin");
    private static readonly int GroundSizeID = Shader.PropertyToID("_GroundSize");

    private void Awake()
    {
        planeRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (splatMap == null)
            return;

        Vector3 scale = transform.lossyScale;

        propertyBlock.SetTexture(SplatMapID, splatMap);
        propertyBlock.SetVector(GroundOriginID, new Vector2(transform.position.x, transform.position.z));
        propertyBlock.SetVector(GroundSizeID, new Vector2(scale.x, scale.z));

        planeRenderer.SetPropertyBlock(propertyBlock);
    }
}