using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateMesh(float[,] heightMap, float heightMultiplicate, AnimationCurve meshHeightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Le restamos uno porque recuerden que la cuenta comienza de 0, aparte lo dividimos por -2 para que nos de la mitad del map pero en negativo
        float halfWidth = (width - 1) / 2f;
        float halfHeight  = (height - 1) / 2;

        // Este es el numero por el que dividimos la cantidad de vertices y lo usamos para ir aumentando la posicion de la X e Y.
        int factorNumber = levelOfDetail == 0? 1 : levelOfDetail * 2;

        // Este lo usamos para indicarle al mesh la cantidad de vertices
        int verticesPerLine = (width - 1) / factorNumber + 1;

        int vertexIndex = 0;
        
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);

        // Aca si hacemos que sea y < vertices per line entonces la posicion de x e y seguira sumando de a 1 simplemente, entonces en vez de eso, vamos sumando el factor entonces cada uno se pone en la posicion que va
        for(int y=0; y<height; y += factorNumber)
        {
            for(int x=0; x<width; x += factorNumber)
            {
                // ACA LE DAMOS EL TAMAÑO AL MESH, NO IMPORTA EL LEVEL OF DETAIL, aqui se le asigna una posicion a cada vertice
                meshData.vertices[vertexIndex] = new Vector3(x - halfWidth, meshHeightCurve.Evaluate(heightMap[x,y]) *heightMultiplicate, y - halfHeight);

                // hacemos el mapa uvs que siempre tiene que ser desde 0 a 1, asi que dividimos por el widht y el height
                // y lo hacemos en un array al mapa uvs ya que para hacer el mesh nos lo pide en una array de vector2, lo mismo que los vertices
                meshData.uvs[vertexIndex] = new Vector2(1f - (float)x/width, 1f - (float)y/height);

                if(x < width-1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine, vertexIndex + verticesPerLine + 1);
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    // Creamos la variable para cada vertice, que se guardara en un array UNIDIMENSIONAL, pero contendra los datos de donde se encuentra el vertice y su altura
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth -1) * (meshHeight -1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;
        triangleIndex += 3;
    }

    // Función para crear el mesh a partir de todo lo que hicimos hasta ahora
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        return mesh;
    }
}