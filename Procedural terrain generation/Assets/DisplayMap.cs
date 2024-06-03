using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    public Renderer textureRender;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public Texture2D DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        //Creamos el colorMap
        Color[] colorMap = new Color[width * height];

        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {   
                //Tomamos el inicio de la fila y le sumamos la columna donde esta el pixel osea X
                colorMap[y * width + x] = Color.Lerp(Color.blue, Color.green, noiseMap[x,y]);
            }
        }
        
        //Ahora le ponemos el colourMap a la textura
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public Texture2D DrawColourMap(float[,] noiseMap, Terrain[] regions)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;

        //Creamos el colorMap
        Color[] colorMap = new Color[width * height];

        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {   
                for(int i = 0; i < regions.Length; i++)
                {
                    if(regions[i].height >= noiseMap[x,y])
                    {
                        colorMap[y * width + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        
        //Ahora le ponemos el colourMap a la textura
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }
    
    public void DrawTexture(Texture2D texture)
    {
        //Usamos Sharedmaterial para poder generar la textura desde el editor y no tener que iniciar el juego para ver los mapas
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
