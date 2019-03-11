using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject floorTile;
    public GameObject player;
    public int boardSize;
    public float waveHeight = 3f;
    public float waveSpeed = .01f;
    public float waveDuration = 4f;
    public float betweenWaves = .3f;

    private List<GameObject> floorTiles = new List<GameObject>();
    private List<Wave> waves = new List<Wave>();

    /// <summary>
    /// Describes an wave on the floor
    /// </summary>
    private class Wave
    {
        private float time = 0f;
        private Vector2 origin;
        public const float scale = 6.3f;

        public Wave(Vector2 position)
        {
            this.origin = position;
        }

        public void incrementTime(float delta)
        {
            this.time += delta;
        }

        public float WaveFunction(float x, float z)
        {
            float distFromRipple = Mathf.Abs(this.time - Mathf.Sqrt(x * x + z * z));
            return 1 / Mathf.Pow((1 + distFromRipple), 3);
        }

        public Vector3 getOrigin()
        {
            return new Vector3(origin.x, origin.y, .6f);
        }
    }
    
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
                Vector3 position = new Vector3(i*.5f, j*.5f, .6f);
                GameObject tile = Instantiate(floorTile, position, Quaternion.identity);
                tile.name = "Tile " + i + ", " + j;
                tile.transform.parent = transform;
                floorTiles.Add(tile);
            }
        }
    }

    IEnumerator CreateWave(Vector2 position)
    {
        Wave w = new Wave(position);
        waves.Add(w);

        yield return new WaitForSeconds(betweenWaves);
        Wave w2 = new Wave(position);
        waves.Add(w2);

        //yield return new WaitForSeconds(betweenWaves);
        //Wave w3 = new Wave(position);
        //waves.Add(w3);

        yield return new WaitForSeconds(waveDuration);
        waves.Remove(w);

        yield return new WaitForSeconds(betweenWaves);
        waves.Remove(w2);

        //yield return new WaitForSeconds(betweenWaves);
        //waves.Remove(w3);
    }

    /// <summary>
    /// Updates the position of each floor tile with respect to all the active waves
    /// </summary>
    void UpdateFloorTiles()
    {
        foreach (GameObject t in floorTiles)
        {

            float x = t.transform.position.x;
            float y = t.transform.position.y;
            float z = 1;

            foreach (Wave w in waves)
            {
                if (isWaving(t, w))
                {
                    w.incrementTime(waveSpeed * Time.deltaTime);
                    Vector3 waveOrigin = w.getOrigin();
                    z += waveHeight * w.WaveFunction(x - waveOrigin.x, y - waveOrigin.y);
                }
            }

            t.transform.position = new Vector3(x, y, z);
        }
    }

    /// <summary>
    /// Determines if a floor tile should be affected by a wave, or if it is blocked by a wall.
    /// </summary>
    /// <param name="tile">The floor tile in question</param>
    /// <param name="wave">The wave affecting the tile</param>
    /// <returns>Whether or not the tile should be lifted by the wave</returns>
    bool isWaving(GameObject tile, Wave wave)
    {
        int layerMask = 1 << 9;
        RaycastHit hit;
        Vector3 vecToOrigin = wave.getOrigin() - tile.transform.position;
        if (Physics.Raycast(tile.transform.position, wave.getOrigin() - tile.transform.position, out hit, vecToOrigin.magnitude, layerMask))
        {
            return false;
        }
        return true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 playerPos3 = player.transform.position;
            Vector2 playerPos2 = new Vector2(playerPos3.x, playerPos3.y);
            StartCoroutine(CreateWave(playerPos2));
        }

        UpdateFloorTiles();
    }
}
