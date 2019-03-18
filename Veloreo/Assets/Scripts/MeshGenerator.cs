using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int floorSize = 10;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        CreateFloor(floorSize);
        UpdateMesh();
    }

    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        for (int y = 0; y < floorSize; y++)
        {
            for (int x = 0; x < floorSize; x++)
            {
                int i = x + floorSize * y;

                vertices[i] = new Vector3(x, f(x, y), y);
            }
        }

        UpdateMesh();
    }

    float f(int x, int y)
    {
        return Mathf.Sin((x*x + y*y)/3 + timer);
    }

    void CreateFloor(int floorSize)
    {
        vertices = new Vector3[floorSize * floorSize];
        triangles = new int[(floorSize - 1) * (floorSize - 1)*6];
        int triIndex = 0;

        for (int y = 0; y < floorSize; y++)
        {
            for (int x = 0; x < floorSize; x++)
            {
                int i = x + floorSize * y;

                vertices[i] = new Vector3(x, 0, y);

                if (x != floorSize - 1 && y != floorSize - 1)
                {
                    triangles[triIndex] = i + floorSize + 1;
                    triangles[triIndex + 1] = i;
                    triangles[triIndex + 2] = i + floorSize;

                    triangles[triIndex + 3] = i + 1;
                    triangles[triIndex + 4] = i;
                    triangles[triIndex + 5] = i + floorSize + 1;

                    triIndex += 6;
                }
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
