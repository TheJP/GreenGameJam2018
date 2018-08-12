using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaveSpawner))]
public class WaveGenerator : MonoBehaviour
{
    public MonsterAsset spiderAsset;
    public MonsterAsset meteorAsset;
    public MonsterAsset wormAsset;
    public float timeBetweenWaves = 20f;

    public WaveDisplayController waveDisplayCtrl;

    private WaveSpawner waveSpawner;

    private Dictionary<MonsterType, MonsterAsset> monsterAssets;

    private void Awake() => waveSpawner = GetComponent<WaveSpawner>();

    private IEnumerator Start()
    {
        // Define monster assets
        monsterAssets = new Dictionary<MonsterType, MonsterAsset>()
        {
            [MonsterType.Meteor] = meteorAsset,
            [MonsterType.Spider] = spiderAsset,
        };

        Wave testWave = new Wave(monsterAssets[MonsterType.Meteor], 7, 0.8f);
        waveDisplayCtrl.AddWaveEntry(testWave);

        yield return new WaitForSeconds(timeBetweenWaves);

        Wave testWave2 = new Wave(monsterAssets[MonsterType.Spider], 3, 0.5f);
        waveDisplayCtrl.AddWaveEntry(testWave2);
    }

}
