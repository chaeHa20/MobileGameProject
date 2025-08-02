using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class DDSAssetToPNGConverter : MonoBehaviour
{
    public bool export;
    public Texture2D TEX1;


    void Update()
    {
        if (export)
        {
            CopyAndSavePNG();
            export = false;
        }
    }

    
    void CopyAndSavePNG()
    {
        Texture2D tex2 = new Texture2D(TEX1.width,TEX1.height, TEX1.format, true);
        Graphics.CopyTexture(TEX1, tex2);
        Texture2D tex3 = new Texture2D(TEX1.width,TEX1.height, TextureFormat.RGBA32, true);
        tex3.SetPixels(tex2.GetPixels());

        byte[] bytes = tex3.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + TEX1.name + ".png", bytes);
        Debug.Log("done");
    }
    
}