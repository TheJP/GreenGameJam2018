using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BloodMoon : MonoBehaviour
{
	public event Action<float> BloodMoonAdvancedWarning;
	public event Action BloodMoonAlert;
	
	[SerializeField]
	private float intervalMin = 150;

	[SerializeField]
	private float intervalMax = 300;

	[SerializeField]
	[Tooltip("Make sure it is large enough for them to complete the planet system minigame (e.g. 30s+)")]
	private float advancedWarningTime = 50;

	[SerializeField]
	[Tooltip("Audio to play as warning before blood moon")]
	private AudioSource audioSourceWarning;

	[SerializeField]
	[Tooltip("Audio to play when blood moon occurs")]
	private AudioSource audioSourceBloodMoon;

	
	private IEnumerator currentBloodMoon;

	private void Start()
	{
		StartBloodMoonRoutine();		
	}

	private IEnumerator NextBloodMoonEvent(float secondsUntil)
	{
		yield return new WaitForSeconds(secondsUntil);
		
		// Action station, action station
		// Blood moon might be happening soon, give out alerts, man the stations
		BloodMoonAdvancedWarning?.Invoke(advancedWarningTime);
		audioSourceWarning.Play();
		
		yield return new WaitForSeconds(advancedWarningTime);
		
		// Too late, blood moon is now happening
		audioSourceBloodMoon.Play();
		BloodMoonAlert?.Invoke();
		
		StartBloodMoonRoutine();
	}

	private void StartBloodMoonRoutine()
	{
		var interval = Random.Range(intervalMin, intervalMax);
		currentBloodMoon = NextBloodMoonEvent(interval);
		StartCoroutine(currentBloodMoon);
	}

	public void CancelBloodMoon()
	{
		if (currentBloodMoon != null)
		{
			StopCoroutine(currentBloodMoon);
			StartBloodMoonRoutine();
		}
	}
}
