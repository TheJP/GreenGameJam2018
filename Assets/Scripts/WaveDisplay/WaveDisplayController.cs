using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveDisplayController : MonoBehaviour {

	public GameObject enemyCountDisplayPrefab;
	public int distance;
	public int numberOfPlaces;
	
	// public GameObject[] m_Displays;   

	private Wave[] waves;
	private int offsetPlaces;		// zeitabhängig..., besser lösen
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		// do someting about keeping track of insertion point or wait time for next free space in display?
	}

	public void AddWaveEntry(Wave wave)
	{
		
		GameObject display = Instantiate(enemyCountDisplayPrefab, this.transform.position - Vector3.down * numberOfPlaces * distance, this.transform.rotation);
		MonsterAsset asset = wave.Asset;

		Debug.Log("AddWaveEntry: asset is " + asset);
		display.name = "Wave: " + wave.Count + " of type " + wave.Asset.monsterName;
		display.transform.SetParent(this.transform, true);
		Debug.Log("have added display: " + display.name);
		
		Text text = display.GetComponentInChildren<Text>();
		text.text = "" + wave.Count;
		text.color = wave.Asset.textColor;
		
		Image image = display.GetComponentInChildren<Image>();
		image.sprite = wave.Asset.icon;
	}
	
}
