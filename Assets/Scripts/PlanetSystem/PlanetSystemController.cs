using System;
using System.Collections;
using System.Linq;
using Boo.Lang;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlanetSystemController : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField]
	private GameObject earth;
	
	[SerializeField]
	private GameObject moon;
	
	[SerializeField]
	private GameObject arcRenderHelper;

	[SerializeField]
	private float angularSpeed;

	[SerializeField]
	private SpriteRenderer[] boxSpriteRenderers;

	[SerializeField]
	private CrosshairController[] playerCrosshairs;

	[SerializeField]
	private Text countdownText;
#pragma warning restore 0649

	private LineRenderer lineRenderer;
	private SpriteRenderer moonSpriteRenderer;

	private float currentAngleStart = Single.NaN;
	private float currentAngleEnd = Single.NaN;

	private PlayerFactory playerFactory;
	private float countdown;
	private bool failed;

	private Vector3 moonOrigin;
	
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		moonSpriteRenderer = moon.GetComponent<SpriteRenderer>();

		playerFactory = FindObjectOfType<PlayerFactory>();

		moonOrigin = moon.transform.position;

		foreach (var crosshair in playerCrosshairs)
		{
			crosshair.gameObject.SetActive(false);
		}

//		CreateBloodMoonEvent(TestIt);
	}

//	private void TestIt(bool success)
//	{
//		Debug.Log(success ? "Success" : "Failure");
//		CreateBloodMoonEvent(TestIt);
//	}
	
	private void Update()
	{
		countdown = Mathf.Max(countdown - Time.deltaTime, 0);
		countdownText.text = $"{countdown:0}s";

		if (countdown < Single.Epsilon)
		{
			return;
		}
		
		moon.transform.RotateAround(earth.transform.position, Vector3.forward, angularSpeed * Time.deltaTime);

		if (Single.IsNaN(currentAngleStart) || Single.IsNaN(currentAngleEnd))
		{
			return;
		}
		
		var angle = Vector2.SignedAngle(Vector2.right, moon.transform.position - earth.transform.position);
		if (angle < 0)
		{
			angle = 360 + angle;
		}

		var distanceToStart = (currentAngleStart - angle + 360) % 360.0f;
		var distanceToEnd = (currentAngleEnd - angle + 360) % 360.0f;
		if (distanceToStart <= 10)
		{
			var moonColor = Color.Lerp(Color.red, Color.white, distanceToStart / 10.0f);
			moonSpriteRenderer.color = moonColor;
		}
		else if (distanceToEnd <= 10)
		{
			var moonColor = Color.Lerp(Color.white, Color.red, distanceToEnd / 10.0f);
			moonSpriteRenderer.color = moonColor;
		}

		if (!failed && distanceToEnd < (currentAngleEnd - currentAngleStart + 360) % 360.0f)
		{
			failed = true;
		}
	}

	/// <summary>
	/// Creates a new blood moon event.
	/// </summary>
	/// <param name="callback">This callback will be called when the event finisheds in some way. The parameter will be
	/// true when the player did successful; otherwise false.</param>
	/// <returns>If the event could be started true; otherwise in case one is already running false.</returns>
	public bool CreateBloodMoonEvent(Action<bool> callback)
	{
		if (!Single.IsNaN(currentAngleStart) && !Single.IsNaN(currentAngleEnd))
		{
			return false;
		}
		
		failed = false;
		moonSpriteRenderer.color = Color.white;
		var maxPlayers = Mathf.Min(playerCrosshairs.Length, boxSpriteRenderers.Length);
		var players = playerFactory.GetComponentsInChildren<Player>();

		if (players.Length > maxPlayers)
		{
			var chosenPlayers = new Player[maxPlayers];
			for (var i = 0; i < maxPlayers; ++i)
			{
				var playerIndex = Random.Range(0, players.Length);
				var player = players[playerIndex];
				if (player == null)
				{
					--i;
					continue;
				}

				chosenPlayers[i] = player;
				players[playerIndex] = null;
			}

			players = chosenPlayers;
		}

		for (var i = 0; i < playerCrosshairs.Length; ++i)
		{
			if (i < players.Length)
			{
				playerCrosshairs[i].CrosshairColor = players[i].Color;
				playerCrosshairs[i].PlayerIndex = players[i].PlayerNumber;
				playerCrosshairs[i].gameObject.SetActive(true);

				playerCrosshairs[i].transform.position = earth.transform.position;
				playerCrosshairs[i].transform
					.Translate(new Vector3(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 4.0f), 0));
			}
			else
			{
				playerCrosshairs[i].gameObject.SetActive(false);
			}
		}
		
		foreach (var box in boxSpriteRenderers)
		{
			box.color = new Color(0, 0, 0, 0);
		}

		var start = Random.Range(0, 359);
		var distance = Random.Range(50, 70);
		
		DrawArc(start, distance, 10);

		moon.transform.position = moonOrigin;
		moon.transform.rotation = Quaternion.identity;
		
		var moonStartAngle = (360 + start - angularSpeed * 10) % 360.0f;
		moon.transform.RotateAround(earth.transform.position, Vector3.forward, moonStartAngle);

		countdown = 30;
		
		StartCoroutine(ChallengePlayers(players, callback));

		return true;
	}

	private void DrawArc(float startAngle, float distance, int steps)
	{
		lineRenderer.positionCount = 0;
		
		var angleStep = distance / steps;
		
		arcRenderHelper.transform.position = moonOrigin;
		arcRenderHelper.transform.rotation = Quaternion.identity;
		arcRenderHelper.transform.RotateAround(earth.transform.position, Vector3.forward, startAngle);
		lineRenderer.positionCount = steps + 1;
		lineRenderer.SetPosition(0, arcRenderHelper.transform.position);

		for (var i = 0; i < steps; ++i)
		{
			arcRenderHelper.transform.RotateAround(earth.transform.position, Vector3.forward, angleStep);
			lineRenderer.SetPosition(i + 1, arcRenderHelper.transform.position);
		}

		currentAngleStart = startAngle;
		currentAngleEnd = (startAngle + distance) % 360.0f;
	}

	private IEnumerator ChallengePlayers(Player[] players, Action<bool> callback)
	{
		yield return new WaitForSeconds(1);
		
		var regularSpeed = angularSpeed;
		while (!failed && countdown > Single.Epsilon)
		{
			angularSpeed = regularSpeed;

			var boxes = new List<SpriteRenderer>(boxSpriteRenderers);
			foreach (var player in players)
			{
				var index = Random.Range(0, boxes.Count);
				boxes[index].color = player.Color;
				boxes.RemoveAt(index);
			}

			foreach (var box in boxes)
			{
				box.color = new Color(0, 0, 0, 0);
			}

			yield return null;

			while (!failed && !playerCrosshairs.All(crosshair => !crosshair.gameObject.activeInHierarchy || crosshair.HasValidHit))
			{
				yield return null;
			}

			foreach (var boxSpriteRenderer in boxSpriteRenderers)
			{
				boxSpriteRenderer.color = new Color(0, 0, 0, 0);
			}

			if (!failed)
			{
				angularSpeed /= 10;
			}

			if (!failed && countdown >= Single.Epsilon)
			{
				yield return new WaitForSeconds(Math.Min(8, countdown));
			}
		}

		if (countdown > Single.Epsilon)
		{
			yield return new WaitForSeconds(countdown);
		}

		foreach (var crosshair in playerCrosshairs)
		{
			crosshair.gameObject.SetActive(false);
		}

		countdown = 0;
		currentAngleStart = Single.NaN;
		currentAngleEnd = Single.NaN;
		angularSpeed = regularSpeed;
		
		callback(!failed);
	}
}
