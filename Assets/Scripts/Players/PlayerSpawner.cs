using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Tooltip("Player prefab that is used to spawn players")]
    public Player playerPrefab;

    [Tooltip("Spawnpoints where players can spawn")]
    public Transform[] playerSpawns;

    [Tooltip("Game node that contains all players")]
    public Transform playersParent;

    private readonly List<Player> players = new List<Player>();

    public IReadOnlyList<Player> Players => players.AsReadOnly();

    public PlayerSpawner()
    {
        // Add player one if no player is present
        // Used for testing when the scene is started directly
        if (Settings.JoinedPlayers.Count < 1)
        {
            Settings.AddPlayer(Color.green, 1);
        }
    }

    private void Start()
    {
        players.Clear();
        var joinedPlayers = Settings.JoinedPlayers;
        Debug.Assert(joinedPlayers.Count <= playerSpawns.Length);

        for(int i = 0; i < joinedPlayers.Count; ++i)
        {
            var player = Instantiate(playerPrefab, playersParent);
            player.PlayerNumber = joinedPlayers[i].PlayerNumber;
            player.Color = joinedPlayers[i].Colour;
            players.Add(player);

            var playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.StartPosition = playerSpawns[i].position;
        }
    }
}
