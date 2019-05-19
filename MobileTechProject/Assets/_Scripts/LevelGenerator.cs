using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will generate spikes, walls, and pickups
public class LevelGenerator : MonoBehaviour
{
    public int Difficulty { get; set; }

    #region Hazards
    public GameObject spike;
    public GameObject wall1;
    public GameObject wall0;
    private GameObject wallToSpawn;
    #endregion

    public GameObject player;
    public GameObject fuelPickup;
    public Animator difficultyTextAnim;
    float spikeWidth = 0.5f;

    private Transform playerTrans;
    public float WallHazardPos { get; private set; }
    float latestSpawn = 0;
    int bufferDist = 15;
    public float CeilingHeight { get; private set; }

    private bool shootingSpikes;
    private bool movingWalls;
    private int wallPercentage = 0; // percentrage of time a wall will spawn instead of spikes

    float spikeSpeed = 3.7f;
    float wallMoveDuration = 0.75f;
    public WallBounds wb;

    [SerializeField]
    private FuelManager fm;

    private int spawnSinceLastFuel;
    private int fuelSpawnBuffer;

    private int latestDiffPos;

    int maxClusterNum = 3;

    private void Awake()
    {
        // set height hazards will spawn to correspond to the automatic placement of ceiling and floor
        CeilingHeight = wb.Bound - (spike.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        WallHazardPos = wb.Bound - (wall0.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        // cache player transform
        playerTrans = player.transform;

        // initialise lastSpawn to levelGenerator's starting position
        latestSpawn = (int)transform.position.x;

        spawnSinceLastFuel = 0;
        fuelSpawnBuffer = 70;

        SetDifficulty(0);
        latestDiffPos = 0;
    }

    private void Update()
    {
        if (player != null)
        {
            if (playerTrans.position.x >= latestSpawn - bufferDist) // if the player goes past the last spawned hazard (minus the buffer)
            {
                if (Difficulty < posToNextDiff.Length) // if the current difficulty is lower than the maximum
                {
                    if (playerTrans.position.x >= latestDiffPos + posToNextDiff[Difficulty]) // if the player has passed the last difficulty position, plus the amount of space to get to the next difficulty position
                    {
                        latestDiffPos += posToNextDiff[Difficulty]; // update the most recently passed difficulty position
                        SetDifficulty(Difficulty + 1); // add one to difficulty
                    }
                }

                for (int i = 0; i < 10; i++) // spawn a batch of ten hazards
                {
                    if (Random.Range(0, 101) < wallPercentage) // spawn wall according to percentage of time
                    {
                        SpawnWall(latestSpawn);
                    }
                    else // otherwise spawn a spike
                    {
                        CreateSpikes();
                    }

                    // handle spawning fuel
                    if (latestSpawn >= (spawnSinceLastFuel + fuelSpawnBuffer))
                    {
                        SpawnFuelPickup(latestSpawn - 4);
                        spawnSinceLastFuel = (int)latestSpawn;
                    }
                }
            }
        }
    }

    int hazardGap;
    int rotation;
    float yPos, xPos;
    float updateSpawn;

    void CreateSpikes()
    {
        hazardGap = Random.Range(1, 5); // varying distance between each cluster of hazards
        rotation = Random.Range(0, 2) * 180; // has an equal chance to spawn on floor or ceiling
        yPos = rotation == 0 ? -CeilingHeight : CeilingHeight; // adjust spawn height
        xPos = latestSpawn + hazardGap; // spawn current cluster at last spawn, with a gap in between

        updateSpawn = SpawnCluster(xPos, yPos, rotation) + hazardGap;

        latestSpawn += updateSpawn;
    }

    /// <summary>
    /// Selects one of 3 cluster patterns spikes can spawn in, and spawns them with a random timing offset.
    /// </summary>
    /// <param name="_xPos">X value for spawning.</param>
    /// <param name="_yPos">Y value for spawning.</param>
    /// <param name="_rotation">0 if spawning on floor, 180 if spawning on ceiling.</param>
    /// <returns>Distance until next object can be spawned.</returns>
    float SpawnCluster(float _xPos, float _yPos, int _rotation)
    {
        HazardClusters hazard = null; // initialise value for spike pattern
        float timeOffset = Random.Range(0f, 2.0f); // how long spikes will be on screen before beginning to shoot
        int selection = Random.Range(0, maxClusterNum); // wiich cluster pattern will be assigned

        switch (selection)
        {
            case 0:
                hazard = new HazardClusters.FiveSpike();
                break;

            case 1:
                hazard = new HazardClusters.ThreeSpike();
                break;

            case 2:
                hazard = new HazardClusters.WaveSpike();
                break;

            default:
                hazard = new HazardClusters.WaveSpike();
                Debug.LogWarning("DEFAULT CLUSTER CALLED");
                break;
        }

        // create an parent object to 
        GameObject spikeParent = new GameObject("SpikeParent");
        spikeParent.transform.position = new Vector2(_xPos, _yPos);


        for (int i = 0; i < hazard.SpaceNumber; i++)
        {
            if (hazard.SpawnSpike[i])
            {
                // spawn a spike, iterate the xPos by the number this is in the cluster // add spike component and set delay to value defined in class
                GameObject currSpike = Instantiate(spike, new Vector2(_xPos + (i * spikeWidth), _yPos), Quaternion.Euler(0, 0, _rotation)) as GameObject;

                if (shootingSpikes) // if the difficulty is at 0, the spike will not move, otherwise add the component to allow it to move
                {
                    currSpike.AddComponent<Spike>().Initialize(hazard.Delays[i] + timeOffset, spikeSpeed, playerTrans);
                }

                currSpike.transform.parent = spikeParent.transform;
            }
        }
        spikeParent.AddComponent<HazardProximityDetector>().SetChildSpikes();
        return hazard.SpaceNumber * spikeWidth;
    }

    void SpawnWall(float _xPos)
    {
        int hazardGap = Random.Range(2, 5); // varying distance between each cluster of hazards

        _xPos += hazardGap;
        float yPos = Random.Range(0, 2) == 0 ? -WallHazardPos : WallHazardPos; // either spawn on floor or ceiling

        GameObject currWall = Instantiate(wallToSpawn, new Vector2(_xPos, yPos), transform.rotation);

        if (movingWalls)
        {
            currWall.AddComponent<MoveWall>().Initialise(yPos, -yPos, wallMoveDuration);
        }


        latestSpawn += 8;
    }

    private void SpawnFuelPickup(float _xPos)
    {
        float _yPos = Random.Range(-CeilingHeight + 2, CeilingHeight - 2);
        FuelPickup fPick = Instantiate(fuelPickup, new Vector2(_xPos, _yPos), transform.rotation).GetComponent<FuelPickup>();
        fPick.Initialise(fm, CeilingHeight - 1);
    }

    private int[] posToNextDiff = new int[4] { 75, 100, 200, 200 };
    public void SetDifficulty(int diff)
    {
        bool animate = true;

        Debug.Log("DIFFICULTY UP AT: " + playerTrans.position.x);

        Difficulty = diff;

        // decide what to spawn based on difficulty
        switch (Difficulty)
        {
            case 0:
                animate = false; // don't display "difficulty up" animation for first level

                wallToSpawn = wall0; // spawned walls have no spikes
                movingWalls = false; // walls don't move
                wallPercentage = 40; // 40% chance a hazard will be a wall
                shootingSpikes = false; // spikes don't move
                break;

            case 1: // 75 to get to
                StartCoroutine(wb.DeadlyWalls()); // deadly ceiling and floor
                break;

            case 2: // 100 to get to
                movingWalls = true; // walls move
                wallPercentage = 30; // 30% chance a hazard will be a wall
                shootingSpikes = true; // set spikes to move when player is near enough 
                break;

            case 3: // 200 to get to
                wallToSpawn = wall1; // give walls spikes
                wallPercentage = 25; // 25% chance a hazard will be a wall
                movingWalls = false; // walls no longer move
                // create blocks that shoot spikes off
                break;

            case 4: // 200 to get to
                movingWalls = true; // walls move again
                break;
            default:
                // hopefully this is never reached
                Debug.LogError("DIFFICULTY HAS DEFAULTED");
                break;
        }

        if (animate)
        {
            StartCoroutine(DifficultyAnimation());
        }
    }

    public IEnumerator DifficultyAnimation()
    {
        difficultyTextAnim.SetBool("showing", true);
        yield return new WaitForSeconds(1.5f);
        difficultyTextAnim.SetBool("showing", false);
    }
}
