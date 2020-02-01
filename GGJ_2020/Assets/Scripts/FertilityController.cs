using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilityController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    //================================================================================================================//
    [SerializeField]
    private LayerMask layerMask;
    
    [SerializeField]
    private Material setMaterial;
    
    [SerializeField]
    private Shader drawShader;

    private RenderTexture splatMap;

    private Material drawMaterial;

    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int Coordinate = Shader.PropertyToID("_Coordinate");
    private static readonly int DetailAlbedoMap = Shader.PropertyToID("_DetailAlbedoMap");
    
    //================================================================================================================//

    // Start is called before the first frame update
    private void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector(Color, UnityEngine.Color.red);
        splatMap = new RenderTexture(1024,1024, 0, RenderTextureFormat.ARGBFloat);
        //TODO Need to Set the Detail texture here
        setMaterial.SetTexture(DetailAlbedoMap, splatMap);
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Tab))
            return;

        PaintAt(playerTransform.position);

    }
    
    //================================================================================================================//

    private void PaintAt(Vector3 position)
    {
        if (!Physics.Raycast(position, Vector3.down, out var hit, layerMask.value)) 
            return;

        var texCoord = hit.textureCoord;
        //TODO Raycast down to ground geet hit.textCoord
        drawMaterial.SetVector(Coordinate, new Vector4(texCoord.x,texCoord.y,0f,0f));
        
        var temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height,0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(splatMap, temp);
        Graphics.Blit(temp, splatMap, drawMaterial);
        RenderTexture.ReleaseTemporary(temp);


    }
}
