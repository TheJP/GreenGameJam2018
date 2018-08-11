﻿using System;
using UnityEngine;

public class CountDisplayController : MonoBehaviour
{

	public float m_Speed = 20f;       
	
	
	// Use this for initialization
	void Start () {
		// var display = Instantiate(m_DisplayPrefab, transform.position, transform.rotation);
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.down * m_Speed * Time.deltaTime);
		if (transform.localPosition.y <= 0)
		{
			Debug.Log("spawn monsters!");
			Destroy(gameObject, .5f);
			
			// somehow from the wave:
			// GameObject prefab = wave.Asset.monsterPrefab;	
			// and then spawn the monsters... 
		}
	}
}