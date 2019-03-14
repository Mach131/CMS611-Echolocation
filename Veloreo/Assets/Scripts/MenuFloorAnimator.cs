using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFloorAnimator : MonoBehaviour
{
    public GameObject floorTile;
    public Material mat;
    public int boardSize;
    public float wavePd;

    private List<GameObject> floorTiles = new List<GameObject>();
    private float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        BuildFloor(boardSize);
    }

    /// <summary>
    /// Builds a game board of floor tiles
    /// </summary>
    /// <param name="size">The size of the game board</param>
    void BuildFloor(int size)
    {
        for (int i = -size; i <= size; i++)
        {
            for (int j = -size; j <= size; j++)
            {
                Vector3 position = new Vector3(i * .5f, 0f, j * .5f);
                GameObject tile = Instantiate(floorTile, position, Quaternion.identity);
                tile.name = "Tile " + i + ", " + j;
                tile.transform.parent = transform;
                tile.GetComponent<MeshRenderer>().material = mat;
                floorTiles.Add(tile);
            }
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        foreach (GameObject pt in floorTiles)
        {
            float x = pt.transform.position.x;
            float z = pt.transform.position.z;
            pt.transform.position = new Vector3(x,
                                                f(x, z, time),
                                                z);
        }
    }

    float f(float x, float y, float t)
    {
        //return Mathf.Sin(2 * x + t) / 5;
        //return Mathf.Sin(2*x + t) + Mathf.Cos(2*y + t);
        return Mathf.Sin((x * x + y * y) * wavePd - t) / 6f;
    }
}
