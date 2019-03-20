using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject floorTile;
    public GameObject player;
    public WaveSound waveSoundPlayer;
    public int boardSize;
    public const float defaultWaveHeight = 1f; //1
    public const float defaultWaveSpeed = .02f; //.02
    public const float defaultWaveDuration = 5f; //5
    public const float defaultFallOffResistance = 70f; //70
    public const float defaultSoundFrequency = 440f; //440
    public const float betweenWaves = .3f;
    public float distanceFrom2D = .6f;
    public float cooldownLengthSeconds = 2f;

    private List<GameObject> floorTiles = new List<GameObject>();
    private Guard[] guards;
    private List<Wave> waves = new List<Wave>();
    private Goal goal;
    private GameObject playerString;
    private float cooldownTimer = 0f;

    [SerializeField]
    private int wavesRemaining;
    public static int maximumWaves = 20;
    private WaveText waveText;

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
        private float fallOffResistance = 70f;
        public const float scale = 50.4f;

        public Wave(Vector2 position, float height, float speed, float duration, float resistance = 70f)
        {
            this.origin = position;
            this.waveHeight = height;
            this.waveSpeed = speed;
            this.waveDuration = duration;
            this.fallOffResistance = resistance;
        }

        public void incrementTime(float delta)
        {
            this.time += delta;
        }

        public float WaveFunction(float x, float y)
        {
            float distFromRipple = Mathf.Abs(this.time - Mathf.Sqrt(x * x + y * y));
            //return 1 / (Mathf.Pow((1 + distFromRipple), 3));
            //return Mathf.Pow(2.718f, -distFromRipple / c);
            return Mathf.Pow(2.718f, -(this.time /(this.fallOffResistance*this.waveSpeed))) / (Mathf.Pow(1 + distFromRipple, 3));
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

        public float getTime()
        {
            return this.time;
        }

    }
    
    void Start()
    {
        BuildFloor(boardSize);
        guards = FindObjectsOfType<Guard>();
        goal = FindObjectOfType<Goal>();
        wavesRemaining = maximumWaves;
        waveText = GameObject.Find("WaveCounter").GetComponent<WaveText>();
        playerString = GameObject.FindGameObjectWithTag("string");
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

    IEnumerator CreateWave(Vector2 position, float height = defaultWaveHeight, float speed = defaultWaveSpeed,
        float duration = defaultWaveDuration, float resistance = defaultFallOffResistance, float frequency = defaultSoundFrequency, float waveWait = betweenWaves)
    {
        Wave w = new Wave(position, height, speed, duration, resistance);
        waves.Add(w);

        if (waveSoundPlayer != null)
        {
            // Trying to ensure that the sound ends before another one can be made
            float soundLength = Mathf.Max(0.1f, cooldownLengthSeconds - Time.deltaTime);
            waveSoundPlayer.playSineWave(frequency, soundLength);
        }

        yield return new WaitForSeconds(waveWait);
        Wave w2 = new Wave(position, height, speed, duration, resistance);
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
                    z += w.getHeight() * w.WaveFunction(x - waveOrigin.x, y - waveOrigin.y); //+=

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

    /// <summary>
    /// Makes any guard that is hit by the wave visible.
    /// </summary>
    void UpdateGuardReveals()
    {
        foreach (Guard guard in guards)
        {
            foreach (Wave wave in waves)
            {
                if (isRevealedGuard(guard, wave))
                {
                    Vector2 guardToOrigin = wave.getOrigin() - guard.transform.position;
                    float distance = wave.getTime() / (Wave.scale * wave.getSpeed());
                    // constrain how long to flash so character doesn't stay visible forever
                    if (distance >= .9 * guardToOrigin.magnitude && distance <= 1.25*guardToOrigin.magnitude)
                    {
                        guard.GetComponent<HiddenObject>().Flash();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Determines if a guard should be affected by a wave, or if it is blocked by a wall.
    /// </summary>
    /// <param name="guard">The floor tile in question</param>
    /// <param name="wave">The wave affecting the guard</param>
    /// <returns>Whether or not the guard should be revealed by the wave</returns>
    bool isRevealedGuard(Guard guard, Wave wave)
    {
        Vector2 origin = wave.getOrigin();
        Vector2 guardToOrigin = origin - (Vector2) guard.transform.position;
        // so that we don't collide with ourselves
        float sigma = .05f;
        float minDepth = guard.GetComponent<CircleCollider2D>().radius * guard.transform.localScale.x + sigma;

        Vector2 offset = (Vector2) guard.transform.position + minDepth * guardToOrigin.normalized;
        RaycastHit2D hit = Physics2D.Linecast(offset, origin);
        return (hit.collider == player.GetComponent<CircleCollider2D>());
    }

    private void CheckGoalRevealed()
    {
        foreach (Wave wave in waves)
        {
            if (isRevealedGoal(goal, wave))
            {
                Vector2 goalToOrigin = wave.getOrigin() - goal.transform.position;
                float distance = wave.getTime() / (Wave.scale * wave.getSpeed());
                // constrain how long to flash so character doesn't stay visible forever
                if (distance >= .9 * goalToOrigin.magnitude && distance <= 1.25 * goalToOrigin.magnitude)
                {
                    goal.GetComponent<HiddenObject>().Flash();
                }
            }
        }
    }

    bool isRevealedGoal(Goal goal, Wave wave)
    {
        Vector2 origin = wave.getOrigin();
        Vector2 goalToOrigin = origin - (Vector2) goal.transform.position;
        // so that we don't collide with ourselves
        float sigma = .05f;
        float minDepth = goal.GetComponent<CircleCollider2D>().radius * goal.transform.localScale.x + sigma;
        Vector2 goalPos = goal.transform.position;
        CircleCollider2D playerCollider = player.GetComponent<CircleCollider2D>();
        Vector2 offset;
        RaycastHit2D hit;
        bool collided = false;

        offset = goalPos + goalToOrigin.normalized * minDepth;
        hit = Physics2D.Linecast(offset, origin);
        collided = collided || (hit.collider == playerCollider);

        // check if we hit any corner of the player to make it more reliable
        offset = goalPos + Vector2.up * minDepth;
        hit = Physics2D.Linecast(offset, origin);
        collided = collided || (hit.collider == playerCollider);
        offset = goalPos + Vector2.down * minDepth;
        hit = Physics2D.Linecast(offset, origin);
        collided = collided || (hit.collider == playerCollider);
        offset = goalPos + Vector2.right * minDepth;
        hit = Physics2D.Linecast(offset, origin);
        collided = collided || (hit.collider == playerCollider);
        offset = goalPos + Vector2.left * minDepth;
        hit = Physics2D.Linecast(offset, origin);
        collided = collided || (hit.collider == playerCollider);

        return collided;
    }
    
    void makeWaveByPitch(float pitchNum)
    {
        if (cooldownTimer <= 0 && wavesRemaining > 0)
        {
            Vector3 playerPos3 = player.transform.position;
            Vector2 playerPos2 = new Vector2(playerPos3.x, playerPos3.y);

            float height = defaultWaveHeight;
            float speed = defaultWaveSpeed;
            float duration = defaultWaveDuration;
            float resistance = defaultFallOffResistance;
            float frequency = defaultSoundFrequency;

            switch (pitchNum)
            {
                case 1:
                    height = 10f;
                    speed = .025f;
                    duration = .5f;
                    resistance = 20f;
                    frequency = 392.06f;

                    StartCoroutine(CreateWave(playerPos2, height, speed, duration, resistance, frequency, .1f));
                    vibrateString(height-7, speed, duration);
                    break;
                case 2:
                    height = 1.2f;
                    speed = .025f;
                    duration = 5.5f;
                    resistance = 50f;
                    frequency = 329.66f;

                    StartCoroutine(CreateWave(playerPos2, height, speed, duration, resistance, frequency));
                    vibrateString(height, speed, duration);
                    break;
                case 3:
                    height = .8f;
                    speed = .0225f;
                    duration = 5.5f;
                    resistance = 80f;
                    frequency = 293.67f;

                    StartCoroutine(CreateWave(playerPos2, height, speed, duration, resistance, frequency));
                    vibrateString(height, speed, duration);
                    break;
                case 4:
                    height = .3f;
                    speed = .02f;
                    duration = 6f;
                    resistance = 150f;
                    frequency = 261.63f;

                    StartCoroutine(CreateWave(playerPos2, height, speed, duration, resistance, frequency));
                    vibrateString(height, speed, duration);
                    break;
                case 5:
                    height = .2f;
                    speed = .015f;
                    duration = 6f;
                    resistance = 400f;
                    frequency = 220.00f;

                    StartCoroutine(CreateWave(playerPos2, height, speed, duration, resistance, frequency));
                    vibrateString(height, speed, duration);
                    break;
                default:
                    break;
            }

            //Update some variables
            cooldownTimer = cooldownLengthSeconds;
            wavesRemaining--;
            waveText.changeTextDisplay(wavesRemaining);
        }

    }

    private void vibrateString(float height, float speed, float duration)
    {
        playerString.GetComponent<StringWave>().startVibration(height, speed, duration);
    }

    private bool clickedOnWave()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << 10;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag == "string")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool shouldMakeWave()
    {
        bool noConstraints = cooldownTimer <= 0 && wavesRemaining > 0;
        bool playerActed = Input.GetKeyDown(KeyCode.Space) || clickedOnWave();
        return noConstraints && playerActed;
    }

    void Update()
    {
        if (shouldMakeWave())
        {
            //cooldownTimer = cooldownLengthSeconds;
            //Vector3 playerPos3 = player.transform.position;
            //Vector2 playerPos2 = new Vector2(playerPos3.x, playerPos3.y);
            //StartCoroutine(CreateWave(playerPos2));
            makeWaveByPitch(SliderControls.getValue());
            //wavesRemaining--;
            //waveText.changeTextDisplay(wavesRemaining);
        }
        cooldownTimer -= Time.deltaTime;
        UpdateFloorTiles();
        UpdateGuardReveals();
        CheckGoalRevealed();
    }
}
