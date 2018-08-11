using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    private static readonly List<JoinedPlayer> joinedPlayers = new List<JoinedPlayer>();

    public static IReadOnlyList<JoinedPlayer> JoinedPlayers => joinedPlayers.AsReadOnly();

    public static void AddPlayer(Color colour, int playerNumber) => joinedPlayers.Add(new JoinedPlayer(colour, playerNumber));

    public static void RemovePlayer(JoinedPlayer player) => joinedPlayers.Remove(player);

    public class JoinedPlayer
    {
        public Color Colour { get; }
        public int PlayerNumber { get; }

        public JoinedPlayer(Color colour, int playerNumber)
        {
            Colour = colour;
            PlayerNumber = playerNumber;
        }
    }
}
