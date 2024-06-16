using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode{
        NoiseMap,
        ColourMap,
        Mesh
    }
    public DrawMode drawMode;

    public const int mapChunks = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public float heightMultiplicate;
    public int seed;
    public Vector2 offset;
    
    public AnimationCurve meshHeightCurve;

    public Terrain[] regions;

    public bool autoUpdate;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            GenerateMap();
        }
    }
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunks, mapChunks, noiseScale, seed, octaves, persistance, lacunarity, offset);

        DisplayMap display = FindObjectOfType<DisplayMap>();

        if(drawMode == DrawMode.NoiseMap)
        {
            // Dibujamos la textura a partir la textura que nos devuelve la funcion DrawNoiseMa
            display.DrawTexture(display.DrawNoiseMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(display.DrawColourMap(noiseMap, regions));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, heightMultiplicate, meshHeightCurve, levelOfDetail), display.DrawColourMap(noiseMap, regions));
        }
        
    }

    void OnValidate()
    {
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 1)
        {
            octaves = 1;
        }
    }
}

[System.Serializable]
public struct Terrain{
    public string name;
    public float height;
    public Color color;
}
