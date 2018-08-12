using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Monsters;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
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

        public override string ToString()
        {
            return
                $"{{number: {Number}, name: {Name}, modifier : {Modifier}, enemies: {string.Join(",", Enemies.ToList())}}}";
        }
    }

    public KeyWave[] KeyWaves;
    public Transform[] SpawnPoints;
    public float TimeBetweenWaves = 5f;
    public Tilemap Placeables;
    public Tilemap Terrain;

    public int NextWave => _waveCounter + 1;
    //public float WaveCountdown => _waveCountdown;
    public bool EndlessMode => _keyWaves.Count == 0;
    //public SpawnState State => _state;
    public bool EnemyIsAlive => _enemiesContainer != null && _enemiesContainer.transform.childCount > 0;

    private int _waveCounter = 1;
    private Stack<KeyWave> _keyWaves;
    private KeyWave _lastKeyWave;
    // private float _searchCountdown = 1f; // Not used
    private float _waveCountdown;
    private SpawnState _state = SpawnState.COUNTING;
    private GameObject _enemiesContainer = null;

    private void Awake()
    {
        Debug.Log("resetting waves");
        if (KeyWaves.Length == 0)
        {
            Debug.LogError("No waves defined!");
        }

        if (SpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points referenced.");
        }

        EnsureEnemiesContainerExists();
        _waveCounter = 1;
        _keyWaves = new Stack<KeyWave>(KeyWaves.Reverse());
        _lastKeyWave = null;
        _state = SpawnState.COUNTING;
        //_searchCountdown = 1f;
        _waveCountdown = TimeBetweenWaves;
    }

    void Update()
    {
        if (_state == SpawnState.WAITING)
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
            if (_state != SpawnState.SPAWNING)
            {
                if (!EndlessMode && _waveCounter == _keyWaves.Peek().number)
                {
                    _lastKeyWave = _keyWaves.Pop();
                }

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
        _state = SpawnState.COUNTING;
    }


    private void EnsureEnemiesContainerExists()
    {
        if (_enemiesContainer == null)
        {
            _enemiesContainer = new GameObject();
            _enemiesContainer.transform.parent = transform;
            _enemiesContainer.name = "enemies";
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log($"Spawning Wave: {wave}");
        _state = SpawnState.SPAWNING;
        EnsureEnemiesContainerExists();

        foreach (var enemy in wave.Enemies)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                SpawnEnemy(enemy.Prefab);
                yield return new WaitForSeconds(1f / enemy.Rate);
            }
        }

        _state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Monster prefab)
    {
        Transform _sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
        var enemy = Instantiate(prefab, _sp.position, _sp.rotation);
        enemy.transform.SetParent(_enemiesContainer.transform);
        var monster = enemy.GetComponent<Monster>();
        monster.Placeables = Placeables;
        monster.Terrain = Terrain;
    }
}