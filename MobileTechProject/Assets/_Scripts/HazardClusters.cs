using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file stores clusters of hazards used by LevelGenerator to spawn in pregenerated hazard patterns
public abstract class HazardClusters
{
    public int SpaceNumber { get; private set; }
    public bool[] SpawnSpike { get; private set; }
    public float[] Delays { get; private set; }

    // Five spikes shooting in an arrow formation
    public class FiveSpike : HazardClusters
    {
        public FiveSpike()
        {
            SpaceNumber = 5;
            SpawnSpike = new bool[SpaceNumber];
            Delays = new float[SpaceNumber];

            // is there a spike in this space?
            SpawnSpike[0] = true;
            SpawnSpike[1] = true;
            SpawnSpike[2] = true;
            SpawnSpike[3] = true;
            SpawnSpike[4] = true;

            // time delay of each spike
            Delays[0] = 0.1f;
            Delays[1] = 0.05f;
            Delays[2] = 0.0f;
            Delays[3] = 0.05f;
            Delays[4] = 0.1f;
        }
    }

    // Three spikes shooting in an arrow formation
    public class ThreeSpike : HazardClusters
    {
        public ThreeSpike()
        {
            SpaceNumber = 3;
            SpawnSpike = new bool[SpaceNumber];
            Delays = new float[SpaceNumber];

            // is there a spike in this space?
            SpawnSpike[0] = true;
            SpawnSpike[1] = true;
            SpawnSpike[2] = true;

            // time delay of each spike
            Delays[0] = 0.1f;
            Delays[1] = 0.0f;
            Delays[2] = 0.1f;
        }
    }

    // Three spikes with a gap between two, shooting more delayed between each
    public class WaveSpike : HazardClusters
    {
        public WaveSpike()
        {
            SpaceNumber = 4;
            SpawnSpike = new bool[SpaceNumber];
            Delays = new float[SpaceNumber];

            // is there a spike in this space?
            SpawnSpike[0] = true;
            SpawnSpike[1] = false;
            SpawnSpike[2] = true;
            SpawnSpike[3] = true;

            // time delay of each spike
            Delays[0] = 0.0f;
            Delays[1] = 0.0f;
            Delays[2] = 0.5f;
            Delays[3] = 1.5f;
        }
    }
}