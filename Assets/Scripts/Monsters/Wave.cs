using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    // [SerializeField] private MonsterType monsterType;
    [SerializeField] private MonsterAsset asset;
    [SerializeField] private int count;
    [SerializeField] private float rate;

    // public Wave (MonsterType type, int count, float rate)
    public Wave(MonsterAsset asset, int count, float rate)
    {
        // this.monsterType = type;
        // this.asset = WaveGenerator.GetMonsterAssetForType(type);
        this.asset = asset;
        Debug.Log("Wave.asset is " + this.asset);
        this.count = count;
        this.rate = rate;
    }

    public int Count
    {
        get { return count; }
        set { count = value; }
    }

    public float Rate
    {
        get { return rate; }
        set { rate = value; }
    }

    /*public MonsterType MonsterType
    {
        get { return monsterType; }
        set { monsterType = value; }
    }
    */
    public MonsterAsset Asset
    {
        get { return asset; }
        set { asset = value; }
    }
}
