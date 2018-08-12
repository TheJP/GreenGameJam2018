using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Tooltip("Tile controller that is used to spawn all placeables")]
    public TileController controller;

    [Header("Locations")]

    [Tooltip("Location to spawn the main base tower")]
    public Transform oxygenTower;

    [Tooltip("Locations to spawn the main base trees")]
    public Transform[] oxygenProducer;

    [Tooltip("Locations to spawn the main base solar arrays")]
    public Transform[] solarArrays;

    [Header("Prefabs")]

    [Tooltip("Prefab used to spawn the oxygen tower")]
    public OxygenTower oxygenTowerPrefab;

    [Tooltip("Prefab used to spawn the tree")]
    public OxygenProducer oxygenProducerPrefab;

    [Tooltip("Prefab used to spawn the solar array1")]
    public SolarArray solarArrayPrefab;
}
