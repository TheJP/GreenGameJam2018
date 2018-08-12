using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monsters;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        Spawning,
        Waiting,
        Counting,
    };

    [System.Serializable]
    public class KeyWave
    {
        public string name = "Wave 1";
        public int number = 1;
        public Enemy[] enemies;

        public Modifiers subsequent_rounds;
    }

    [System.Serializable]
    public class Enemy
    {
        public Monster Prefab;
        public int Count = 1;
        public float Rate = 1f;

        public override string ToString()
        {
            return $"{{prefab : {Prefab.gameObject.name}, count : {Count}, rate : {Rate}}}";
        }
    }

    [System.Serializable]
    public class Modifiers
    {
        public float attack = 1f;
        public float health = 1f;

        public float next_attack = 0.05f;
        public float next_health = 0.05f;
        public float next_numbers = 0.10f;
    }

    public class Wave
    {
        private string Name { get; }
        public Enemy[] Enemies { get; }
        public int Number { get; }
        public int Modifier { get; }

        public Wave(int number, KeyWave keyWave)
        {
            Number = number;
            Modifier = (number - keyWave.number);
            Name = keyWave.name;

            Enemies = new Enemy[keyWave.enemies.Length];
            for (int i = 0; i < Enemies.Length; i++)
            {
                var e = new Enemy();
                var enemies = keyWave.enemies[i].Count;
                e.Count = enemies + Mathf.RoundToInt(Modifier * keyWave.subsequent_rounds.next_numbers * enemies);
                e.Prefab = keyWave.enemies[i].Prefab;
                e.Rate = keyWave.enemies[i].Rate;
                Enemies[i] = e;
            }
        }

        public override string ToString() => $"{{number: {Number}, name: {Name}, modifier : {Modifier}, enemies: {string.Join(",", Enemies.ToList())}}}";
    }

    public KeyWave[] keyWaves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;
    public Tilemap placeables;
    public Tilemap terrain;
    public GameObject enemiesContainer = null;

    public int NextWave => _waveCounter + 1;
    //public float WaveCountdown => _waveCountdown;
    //public SpawnState State => _state;
    public bool EnemyIsAlive => enemiesContainer != null && enemiesContainer.transform.childCount > 0;

    private int _waveCounter = 1;
    private KeyWave _lastKeyWave;
    // private float _searchCountdown = 1f; // Not used
    private float _waveCountdown;
    private SpawnState _state = SpawnState.Counting;

    private void Awake()
    {
        if (keyWaves.Length == 0)
        {
            Debug.LogError("No waves defined!");
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");
        }

        EnsureEnemiesContainerExists();
        _waveCounter = 1;
        _lastKeyWave = null;
        _state = SpawnState.Counting;
        //_searchCountdown = 1f;
        _waveCountdown = timeBetweenWaves;

        Array.Sort(keyWaves, (a, b) => a.number.CompareTo(b.number));
    }

    //private Wave GetWave(int number)
    //{
    //}

    private void Start()
    {
        var waves = new Wave[keyWaves.Last().number + 1];

        var keyWave = keyWaves.First();
    }

    void Update()
    {
        if (_state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive)
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }


        if (_waveCountdown <= 0)
        {
            if (_state != SpawnState.Spawning)
            {
                //if (!EndlessMode && _waveCounter == _keyWaves.Peek().number)
                //{
                //    _lastKeyWave = _keyWaves.Pop();
                //}

                StartCoroutine(SpawnWave(new Wave(_waveCounter, _lastKeyWave)));
                _waveCounter++;
            }
        }
        else
        {
            _waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        _state = SpawnState.Counting;
    }


    private void EnsureEnemiesContainerExists()
    {
        if (enemiesContainer == null)
        {
            enemiesContainer = new GameObject();
            enemiesContainer.transform.parent = transform;
            enemiesContainer.name = "enemies";
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log($"Spawning Wave: {wave}");
        _state = SpawnState.Spawning;
        EnsureEnemiesContainerExists();

        foreach (var enemy in wave.Enemies)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                SpawnEnemy(enemy.Prefab);
                yield return new WaitForSeconds(1f / enemy.Rate);
            }
        }

        _state = SpawnState.Waiting;
    }

    public void SpawnEnemy(Monster prefab)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var enemy = Instantiate(prefab, _sp.position, _sp.rotation);
        enemy.transform.SetParent(enemiesContainer.transform);
        var monster = enemy.GetComponent<Monster>();
        monster.Placeables = placeables;
        monster.Terrain = terrain;
    }
}