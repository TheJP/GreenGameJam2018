using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public Tilemap placeables;
    public Tilemap terrain;

    private readonly List<Placeable> placeds = new List<Placeable>();

    private void Awake() => placeds.Clear();

    public void RemovePlaceable(Placeable placeable) => placeds.Remove(placeable);

    public void AddPlaceable(Placeable placeable) => placeds.Add(placeable);

    public bool TryAddTile(TileBase tileType, Vector3 position) => TryAddTile(tileType, placeables.WorldToCell(position));

    public bool TryAddTile(TileBase tileType, Vector3Int position)
    {
        if (terrain.GetTile(position) != null) { return false; }
        if (terrain.GetTile(position + Vector3Int.down) == null) { return false; }
        if (placeables.GetTile(position) != null) { return false; }

        placeables.SetTile(position, tileType);

        return true;
    }

    public IEnumerable<T> GetPlaceablesOfType<T>() where T : Placeable =>
        placeds.Select(p => p as T).Where(p => p != null);
}
