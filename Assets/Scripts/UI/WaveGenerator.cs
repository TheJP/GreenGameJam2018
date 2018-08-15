using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WaveSpawner))]
public class WaveGenerator : MonoBehaviour
{
    public MonsterAsset spiderAsset;
    public MonsterAsset meteorAsset;
    public MonsterAsset wormAsset;

    [Tooltip("Time between the spawning of two waves")]
    public float timeBetweenWaves = 20f;

    [Tooltip("How many seconds in advance a wave is displayed")]
    public float displayInAdvance = 60f;

    public WaveDisplayController waveDisplayCtrl;

    private WaveSpawner waveSpawner;

    private Dictionary<MonsterType, MonsterAsset> monsterAssets;

    private void Awake() => waveSpawner = GetComponent<WaveSpawner>();

    private int waveCount = 0;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;

        // Define monster assets
        monsterAssets = new Dictionary<MonsterType, MonsterAsset>()
        {
            [MonsterType.Meteor] = meteorAsset,
            [MonsterType.Spider] = spiderAsset,
            [MonsterType.Worm] = wormAsset,
        };
    }

    private void Update()
    {
        var timeUntilThisWave = waveCount * timeBetweenWaves - (Time.time - startTime);
        if (timeUntilThisWave < displayInAdvance)
        {
            var wave = waveSpawner.GetWave(waveCount);
            var waveDisplay = new Wave(monsterAssets[wave.Enemies.First().monsterType], wave.Enemies.First().Count, timeUntilThisWave);
            var display = waveDisplayCtrl.AddWaveEntry(waveDisplay);
            display.SpawnWave += () => StartCoroutine(waveSpawner.SpawnWave(wave));
            ++waveCount;
        }
    }

}
