using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private GameObject playerPrefab;
#pragma warning restore 0649
    private const int NumberOfFirstPlayer = 1;
    private List<GameObject> players;

    public Color[] playerColors;
    public GameObject[] playerPositions;

    void Start()
    {
        if(playerColors.Length != playerPositions.Length)
        {
            throw new ArgumentException("Amount of player positions and colours does not match");
        }
        else if (playerPrefab == null)
        {
            throw new MissingReferenceException("Prefab of Player required");
        }
        else
        {
            players = new List<GameObject>(playerColors.Length);
            for (int playerNumber = NumberOfFirstPlayer; playerNumber <= playerColors.Length; playerNumber++)
            {
                GameObject playerObject = Instantiate(playerPrefab, transform);
                Player player = playerObject.GetComponent<Player>();
                player.PlayerNumber = playerNumber;
                player.Color = playerColors[playerNumber - 1];
                player.MaxOxygen = 100;

                PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
                playerMovement.StartPosition = playerPositions[playerNumber - 1].transform.position;

                players.Add(playerObject);
            }
        }
    }
}