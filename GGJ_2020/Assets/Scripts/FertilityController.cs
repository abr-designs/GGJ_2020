using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilityController : MonoBehaviour
{
    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int Coordinate = Shader.PropertyToID("_Coordinate");
    private static readonly int DetailAlbedoMap = Shader.PropertyToID("_Splatter");
    //private static readonly int DetailAlbedoMap2 = Shader.PropertyToID("_DetailAlbedoMap2");
    private static readonly int Brush = Shader.PropertyToID("_Brush");
    


    [SerializeField] private bool isDebug;
    //================================================================================================================//
    [SerializeField]
    private LayerMask layerMask;
    
    [SerializeField]
    private Material setMaterial;
    
    [SerializeField]
    private Shader drawShader;

    private RenderTexture splatMap;

    private Material drawMaterial;

    private readonly float checkDistance = 10f;
    
    //================================================================================================================//

    // Start is called before the first frame update
    private void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector(Color, UnityEngine.Color.white);
        splatMap = new RenderTexture(1024,1024, 0, RenderTextureFormat.ARGBFloat);
        //TODO Need to Set the Detail texture here
        setMaterial.SetTexture(DetailAlbedoMap, splatMap);
    }

    //================================================================================================================//

    public void PaintAt(Vector3 position, float brushSize = 25f)
    {
        Debug.DrawRay(position, Vector3.down * checkDistance, UnityEngine.Color.blue, 2f);
        if (!Physics.Raycast(position, Vector3.down * checkDistance, out var hit, layerMask.value)) 
            return;

        
        var texCoord = hit.textureCoord;
        //TODO Raycast down to ground geet hit.textCoord
        drawMaterial.SetVector(Coordinate, new Vector4(texCoord.x,texCoord.y,0f,0f));
        drawMaterial.SetFloat(Brush, brushSize);
        
        var temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height,0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(splatMap, temp);
        Graphics.Blit(temp, splatMap, drawMaterial);
        RenderTexture.ReleaseTemporary(temp);

        setMaterial.SetTexture(DetailAlbedoMap, splatMap);

        //Debug.Log($"Hit {hit.collider.gameObject.name} at Tex Coord [{texCoord}]");
        //Debug.Break();
    }

    private void OnGUI()
    {
        if(!isDebug)
            return;
        
        GUI.DrawTexture(new Rect(0,0,256,256), splatMap, ScaleMode.ScaleToFit, false, 1);
    }
}
