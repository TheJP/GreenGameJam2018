using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{

    public MonsterAsset spiderAsset;
    public MonsterAsset meteorAsset;
    // public MonsterAsset wormAsset;
    public int waveTimeMinDistanceSec = 5;

    public WaveDisplayController waveDisplayCtrl;

    private Dictionary<MonsterType, MonsterAsset> monsterAssets;

    private IEnumerator Start()
    {
        // Define monster assets
        monsterAssets = new Dictionary<MonsterType, MonsterAsset>()
        {
            [MonsterType.Meteor] = meteorAsset,
            [MonsterType.Spider] = spiderAsset,
        };

        // Nur für Test
        Wave testWave = new Wave(monsterAssets[MonsterType.Meteor], 7, 0.8f);
        waveDisplayCtrl.AddWaveEntry(testWave);

        yield return new WaitForSecondsRealtime(waveTimeMinDistanceSec);

        Wave testWave2 = new Wave(monsterAssets[MonsterType.Spider], 3, 0.5f);
        waveDisplayCtrl.AddWaveEntry(testWave2);
    }

}
