using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoonCockpit : MonoBehaviour
{
	[SerializeField]
	private BloodMoon bloodMoon;
	
	[SerializeField]
	private GameObject planetMiniGamePrefab;

	private bool cockpitIsActive;
	private Player[] players;
	private GameObject planetMiniGame;
	private PlanetSystemController planetSystemController;
	private float bloodMoonCountdown;

	private readonly Dictionary<int, bool> playersInRange;

	public MoonCockpit()
	{
		playersInRange = new Dictionary<int, bool>();
	}
	
	private void Start ()
	{
		var playerSpawner = FindObjectOfType<PlayerSpawner>();
		players = playerSpawner.Players.ToArray();

		foreach (var player in players)
		{
			playersInRange[player.PlayerNumber] = false;
		}

		planetMiniGame = Instantiate(planetMiniGamePrefab, transform);
		planetSystemController = planetMiniGame.GetComponentInChildren<PlanetSystemController>();
		planetMiniGame.SetActive(false);

		bloodMoon.BloodMoonAdvancedWarning += OnBloodMoonAdvancedWarning;
		bloodMoon.BloodMoonAlert += OnBloodMoon;
	}

	private void FixedUpdate()
	{
		if (cockpitIsActive)
		{
			bloodMoonCountdown -= Time.fixedDeltaTime;
		}
	}

	private void StartCockpit ()
	{
		if (!cockpitIsActive)
		{
			return;
		}

		foreach (var player in playersInRange)
		{
			if (!player.Value)
			{
				return;
			}
		}
		
		planetMiniGame.SetActive(true);
		StartCoroutine(StartBloodMoonEvent());
	}

	private IEnumerator StartBloodMoonEvent()
	{
		foreach (var player in players)
		{
			player.GetComponent<PlayerMovement>().enabled = false;
			player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
		
		yield return null;
		planetSystemController.CreateBloodMoonEvent(success =>
		{
			if (success && cockpitIsActive)
			{
				bloodMoon.CancelBloodMoon();
			}

			cockpitIsActive = false;
			planetMiniGame.SetActive(false);

			foreach (var player in players)
			{
				player.GetComponent<PlayerMovement>().enabled = true;
			}
		}, bloodMoonCountdown);
	}

	private void OnBloodMoonAdvancedWarning(float timeUntilBloodMoon)
	{
		bloodMoonCountdown = timeUntilBloodMoon;
		cockpitIsActive = true;
		StartCockpit();
	}

	private void OnBloodMoon()
	{
		cockpitIsActive = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		var player = other.GetComponent<Player>();
		if (player == null)
		{
			return;
		}

		playersInRange[player.PlayerNumber] = true;
		StartCockpit();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		var player = other.GetComponent<Player>();
		if (player == null)
		{
			return;
		}
		
		playersInRange[player.PlayerNumber] = false;
	}
}
