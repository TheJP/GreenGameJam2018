using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public MonsterAsset Asset { get; }
    public int Count { get; }
    public float TimeUntilThisWave { get; }

    public Wave(MonsterAsset asset, int count, float timeUntilThisWave)
    {
        Asset = asset;
        Count = count;
        TimeUntilThisWave = timeUntilThisWave;
    }
}
