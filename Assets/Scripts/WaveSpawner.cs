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
    public Tilemap placeables;
    public Tilemap terrain;
    public GameObject enemiesContainer = null;

    private SpawnState state = SpawnState.Waiting;

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
        state = SpawnState.Waiting;

        Array.Sort(keyWaves, (a, b) => a.number.CompareTo(b.number));
    }

    public Wave GetWave(int number) => new Wave(number, keyWaves.First(w => w.number <= number));

    private void EnsureEnemiesContainerExists()
    {
        if (enemiesContainer == null)
        {
            enemiesContainer = new GameObject();
            enemiesContainer.transform.parent = transform;
            enemiesContainer.name = "enemies";
        }
    }

    public IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.Spawning;
        EnsureEnemiesContainerExists();

        foreach (var enemy in wave.Enemies)
        {
            for (int i = 0; i < enemy.Count; i++)
            {
                SpawnEnemy(enemy.Prefab);
                yield return new WaitForSeconds(1f / enemy.Rate);
            }
        }

        state = SpawnState.Waiting;
    }

    private void SpawnEnemy(Monster prefab)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var enemy = Instantiate(prefab, _sp.position, _sp.rotation);
        enemy.transform.SetParent(enemiesContainer.transform);
        var monster = enemy.GetComponent<Monster>();
        monster.Placeables = placeables;
        monster.Terrain = terrain;
    }
}