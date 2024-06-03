using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        // Generador de numeros random a travez de semilla, es decir si le ponemos la misma semilla va a generar el mismo numero random
        System.Random prng = new System.Random(seed);

        // Creamos un arreglo para guardar el offset de cada octave
        Vector2[] octavesOffset = new Vector2[octaves];

        // Este for se hace aparte ya que si lo hiciesemos dentro de los demas se haria un numero aleatorio para cada pixel
        for(int i=0; i<octaves; i++)
        {
            // A partir de la semilla ingresada
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffset[i] = new Vector2(offsetX, offsetY);
        }

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        float halfWidht = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        if(scale <= 0f)
        {
            scale = 0.0001f;
        }

        for(int y=0; y<mapHeight; y++){
            for(int x=0; x<mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // creamos un for para cada uno de lo octavos
                for(int i=0; i<octaves; i++)
                {
                    // CUANDO SE AUMENTA EL SCALE SE HACE MAS GRANDE EL PRIMER OCTAVE Y CUANDO SE AUMENTA EL FREQUENCY SE HACEN MAS CHIQUITOS LOS SIGUINTES YA QUE NO HAY NINGUN SIGNO MAS NI NINGUN SIGNO MENOS
                    // ES DECIR, NUNCA SE SEPARA LA OPERACION, EL PRIMER OCTAVO SE DIVIDE Y LOS SIGUIENTES SE MULTIPLICA EL NUMERO

                    // Hacemos que empieze desde la mitad de la pantalla a armar el noisemap para hacerle zoom al medio
                    float sampleX = (x-halfHeight) / scale * frequency + octavesOffset[i].x;
                    float sampleY = (y-halfHeight) / scale * frequency + octavesOffset[i].y;

                    // valor perlin, el perlin noise es un patron pseudoaleatorio que simula un patron natural, ideal para mapas o terrenos
                    // convertimos el perlin noise en un rango de -1 a 1 para restarle al octavo principal cuando toque menos, y no sea solo hacia arriba, ya luego normalizamos de 0 a 1 de nuevo
                    float perlinValue = Mathf.PerlinNoise(sampleX,sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    // Modificamos la amplitude y la frecuencia para el siguiente octavo
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                // trackeamos el punto maximo y el minimo para luego normalizarlo a 0 y 1 y poner el punto maximo y minimo como limites, osea como 0 y 1
                if(noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }
                if(noiseHeight > maxHeight)
                {
                    maxHeight = noiseHeight;
                }

                // Luego de pasar por todos los octavos ahora si podemos poner la altura de este pixel
                noiseMap[x,y] = noiseHeight;
            }
        }


        // Ahora normalizamos de nuevo el noise map entre 0 y 1 con inverselerp que devuelve un valor entre 0 y 1
        for(int y=0; y<mapHeight; y++){
            for(int x=0; x<mapWidth; x++)
            {
                noiseMap[x,y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x,y]);
            }
        }

        return noiseMap;
    }
}
