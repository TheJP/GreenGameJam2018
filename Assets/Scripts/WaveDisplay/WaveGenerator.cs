using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

    public MonsterAsset spiderAsset;
	public MonsterAsset meteorAsset;
	// public MonsterAsset wormAsset;
	public int waveTimeMinDistanceSec = 5;

	public WaveDisplayController waveDisplayCtrl;	
	

	// Use this for initialization
	void Start () {
		
		// Nur für Test
		Wave testWave = new Wave (GetMonsterAssetForType(MonsterType.Meteor), 7, 0.8f);
		waveDisplayCtrl.AddWaveEntry (testWave);
		
		StartCoroutine(CoolDownNextWave());

	}
	

	private IEnumerator CoolDownNextWave()
	{
		yield return new WaitForSecondsRealtime(waveTimeMinDistanceSec);
		Debug.Log("next wave");
		Wave testWave = new Wave (GetMonsterAssetForType(MonsterType.Spider), 3, 0.5f);
		waveDisplayCtrl.AddWaveEntry (testWave);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public MonsterAsset GetMonsterAssetForType (MonsterType type) 
	{
		MonsterAsset asset = null;
		switch (type)	
        {
            case MonsterType.Spider:
                asset = spiderAsset;
                break;
	        // case MonsterType.Worm:
				// asset = wormAsset; 
                // break;
            case MonsterType.Meteor:
                asset = meteorAsset;
                break;
            default:
                Debug.Log("Monster type unknown: " + type.ToString());
                break;
        }

		Debug.Log("asset is " + asset);
		return asset;
	}

}
