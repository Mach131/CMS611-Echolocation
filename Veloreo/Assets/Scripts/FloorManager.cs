using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject floorTile;
    public GameObject player;
    public int boardSize;
    public const float defaultWaveHeight = 1f;
    public const float defaultWaveSpeed = .02f;
    public const float defaultWaveDuration = 5f;
    public float betweenWaves = .3f;
    public float distanceFrom2D = .6f;
    public float cooldownLengthSeconds = 2f;

    private List<GameObject> floorTiles = new List<GameObject>();
    private List<Wave> waves = new List<Wave>();
    private float cooldownTimer = 0f;

    /// <summary>
    /// Describes an wave on the floor
    /// </summary>
    private class Wave
    {
        private float time = 0f;
        private Vector2 origin;
        private float waveHeight = 1f;
        private float waveSpeed = .02f;
        private float waveDuration = 5f;
        //public const float scale = 6.3f;

        public Wave(Vector2 position, float height, float speed, float duration)
        {
            this.origin = position;
            this.waveHeight = height;
            this.waveSpeed = speed;
            this.waveDuration = duration;
        }

        public void incrementTime(float delta)
        {
            this.time += delta;
        }

        public float WaveFunction(float x, float y)
        {
            float distFromRipple = Mathf.Abs(this.time - Mathf.Sqrt(x * x + y * y));
            return 1 / Mathf.Pow((1 + distFromRipple), 3);
        }

        public Vector3 getOrigin()
        {
            //Trying 0 here, may be broken
            //Currently doesnt break anything, it'll stay.
            return new Vector3(origin.x, origin.y, 0);
        }

        public float getHeight()
        {
            return this.waveHeight;
        }

        public float getSpeed()
        {
            return this.waveSpeed;
        }

        public float getDuration()
        {
            return this.waveDuration;
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
                Vector3 position = new Vector3(i*.5f, j*.5f, distanceFrom2D);
                GameObject tile = Instantiate(floorTile, position, Quaternion.identity);
                tile.name = "Tile " + i + ", " + j;
                tile.transform.parent = transform;
                floorTiles.Add(tile);
            }
        }
    }

    IEnumerator CreateWave(Vector2 position, float height = defaultWaveHeight, float speed = defaultWaveSpeed, float duration = defaultWaveDuration)
    {
        Wave w = new Wave(position, height, speed, duration);
        waves.Add(w);

        yield return new WaitForSeconds(betweenWaves);
        Wave w2 = new Wave(position, height, speed, duration);
        waves.Add(w2);

        yield return new WaitForSeconds(w.getDuration());
        waves.Remove(w);

        yield return new WaitForSeconds(w2.getDuration());
        waves.Remove(w2);

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
            float z = distanceFrom2D;

            foreach (Wave w in waves)
            {
                if (isWaving(t, w))
                {
                    w.incrementTime(w.getSpeed() * Time.deltaTime);
                    Vector3 waveOrigin = w.getOrigin();
                    z += w.getHeight() * w.WaveFunction(x - waveOrigin.x, y - waveOrigin.y);
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
        Vector3 originToTile = tile.transform.position - wave.getOrigin();

        //if (Physics.Raycast(tile.transform.position, wave.getOrigin() - tile.transform.position, out hit, vecToOrigin.magnitude, layerMask))
        //{
        //    return false;
        //}

        //origin, direction, out hit, distance, layermask
        if (Physics.Raycast(wave.getOrigin(), originToTile, out hit, originToTile.magnitude, layerMask))
        {
            return false;
        }
        return true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0)
        {
            cooldownTimer = cooldownLengthSeconds;
            Vector3 playerPos3 = player.transform.position;
            Vector2 playerPos2 = new Vector2(playerPos3.x, playerPos3.y);
            StartCoroutine(CreateWave(playerPos2));
        }
        cooldownTimer -= Time.deltaTime;
        UpdateFloorTiles();
    }
}
