using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
	[SerializeField]
	private float destroyAfter;
	
	private void Start ()
	{
		StartCoroutine(StartSelftDestructionTimer());
	}

	private IEnumerator StartSelftDestructionTimer()
	{
		yield return new WaitForSeconds(destroyAfter);
		
		Destroy(gameObject);
	}
}
