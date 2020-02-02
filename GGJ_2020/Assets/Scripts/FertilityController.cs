using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilityController : MonoBehaviour
{
    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int Coordinate = Shader.PropertyToID("_Coordinate");

    private static int DetailAlbedoMap;
    private static readonly int Brush = Shader.PropertyToID("_Brush");
    private static readonly int Mult = Shader.PropertyToID("_Mult");

    [SerializeField]
    private string targetProperty;

    [SerializeField]
    private int pow2;

    [SerializeField, Range(1f,1000f)]
    private int Multiplier;

    [SerializeField] private bool isDebug;
    //================================================================================================================//
    [SerializeField]
    private LayerMask layerMask;
    
    [SerializeField]
    private Material setMaterial;

    [SerializeField]
    private Material setFogMaterial;    //mac

    [SerializeField]
    private Shader drawShader;

    //[SerializeField]
    private RenderTexture splatMap;

    private Material drawMaterial;

    private readonly float checkDistance = 10f;
    
    
    //================================================================================================================//

    // Start is called before the first frame update
    private void Start()
    {
        DetailAlbedoMap = Shader.PropertyToID(targetProperty);
        
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector(Color, UnityEngine.Color.white * 0.001f);
        splatMap = new RenderTexture(pow2,pow2, 0, RenderTextureFormat.ARGBFloat);
        //TODO Need to Set the Detail texture here
        setMaterial.SetTexture(DetailAlbedoMap, splatMap);
        setFogMaterial.SetTexture(DetailAlbedoMap, splatMap); //mac


    }

    //================================================================================================================//

    public void PaintAt(Vector3 position, float brushSize = 25f)
    {
        Debug.DrawRay(position, Vector3.down * checkDistance, UnityEngine.Color.blue, 2f);
        
        if (!Physics.Raycast(position, Vector3.down, out var hit, checkDistance, layerMask.value)) 
            return;
        
        //Debug.Log($"Hit {hit.collider.gameObject.name} TexCoord {hit.textureCoord} Hit {hit.point}");
        //Debug.Break();
        
        var texCoord = hit.textureCoord;
        //TODO Raycast down to ground geet hit.textCoord
        drawMaterial.SetVector(Coordinate, new Vector4(texCoord.x,texCoord.y,0f,0f));
        drawMaterial.SetFloat(Brush, brushSize);
        drawMaterial.SetFloat(Mult, Multiplier);
        
        var temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height,0, RenderTextureFormat.ARGBFloat);
        Graphics.Blit(splatMap, temp);
        Graphics.Blit(temp, splatMap, drawMaterial);
        RenderTexture.ReleaseTemporary(temp);

        setMaterial.SetTexture(DetailAlbedoMap, splatMap);
        setFogMaterial.SetTexture(DetailAlbedoMap, splatMap);              //mac

        //Debug.Log($"Hit {hit.collider.gameObject.name} at Tex Coord [{texCoord}]");
        //Debug.Break();
    }

    /// <summary>
    /// Checks splay map at position to see if it has a fertility value
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CanPlantAt(Vector3 position)
    {
        
        if (!Physics.Raycast(position + Vector3.up, Vector3.down, out var hit, checkDistance, layerMask.value)) 
            return false;
        
        var text = new Texture2D(splatMap.width, splatMap.height);
        RenderTexture.active = splatMap;
        text.ReadPixels(new Rect(0, 0, splatMap.width, splatMap.height), 0, 0);
        //text.Apply();

        var coord = hit.textureCoord;
        var color = text.GetPixel(Mathf.RoundToInt(coord.x * pow2), Mathf.RoundToInt(coord.y * pow2));
        
        return color.r > 0;
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        if(!isDebug)
            return;
        
        GUI.DrawTexture(new Rect(0,0,256,256), splatMap, ScaleMode.ScaleToFit, false, 1);
    }
    #endif
}
