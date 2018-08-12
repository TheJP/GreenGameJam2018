using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{
    [Tooltip("Tile controller that is used to spawn all placeables")]
    public TileController controller;

    [Header("Locations")]

    [Tooltip("Location to spawn the main base tower")]
    public Transform oxygenTower;

    [Tooltip("Locations to spawn the main base trees")]
    public Transform[] oxygenProducers;

    [Header("Tiles")]

    [Tooltip("Tile used to spawn the oxygen tower")]
    public TileBase oxygenTowerTile;

    [Tooltip("Tile used to spawn the tree")]
    public TileBase oxygenProducerTile;

    private IEnumerator Start()
    {
        controller.TryAddTile(oxygenTowerTile, oxygenTower.position);

        // Spawn one tree per player
        for(int i = 0; i < Settings.JoinedPlayers.Count; ++i)
        {
            controller.TryAddTile(oxygenProducerTile, oxygenProducers[i].position);
        }

        // Wait until oxygen tower is spawned in
        yield return null;

        // Modify base tower to be better and bigger than the regular one
        var tower = FindObjectOfType<OxygenTower>();
        tower.aoeRadius *= 2;
        tower.oxygenRefillPerSecond *= Settings.JoinedPlayers.Count;
        tower.minOxygenUsagePerSecond *= Settings.JoinedPlayers.Count;
    }
}
