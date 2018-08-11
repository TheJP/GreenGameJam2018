using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
	private float destroyAfter;
#pragma warning restore 0649

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
