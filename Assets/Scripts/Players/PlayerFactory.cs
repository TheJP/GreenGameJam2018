using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour
{

    #pragma warning disable 0649
    [SerializeField]
    private GameObject playerPrefab;
    #pragma warning restore 0649
    private readonly int NUMBER_OF_PLAYERS = 4;
    private readonly int NUMBER_OF_FIRST_PLAYER = 1;
    private List<GameObject> players;

    public Color[] playerColors;
    public GameObject[] playerPositions;

    void Start()
    {
        if (playerPrefab == null)
        {
            throw new MissingReferenceException("Prefab of Player required");
        }
        else
        {
            players = new List<GameObject>(NUMBER_OF_PLAYERS);
            for (int playerNumber = NUMBER_OF_FIRST_PLAYER; playerNumber <= NUMBER_OF_PLAYERS; playerNumber++)
            {
                GameObject playerObject = Instantiate(playerPrefab, transform);
                Player player = playerObject.GetComponent<Player>();
                player.PlayerNumber = playerNumber;
                player.Color = playerColors[playerNumber - 1];
                player.MaxOxygen = 100;

                PlayerMovement playerMovement = playerObject.GetComponent<PlayerMovement>();
                playerMovement.StartPosition = playerPositions[playerNumber - 1].transform.position;

                playerMovement.Initilaize();
                player.Initialize();

                players.Add(playerObject);
            }
        }
    }
}