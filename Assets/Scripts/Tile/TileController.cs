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

    public bool TryRemoveTile(Vector3 position, out PrefabTile tile) =>
        TryRemoveTile(placeables.WorldToCell(position), out tile);

    public bool TryRemoveTile(Vector3Int position, out PrefabTile tile)
    {
        tile = placeables.GetTile(position) as PrefabTile;
        if (tile == null) { return false; }
        else
        {
            placeables.SetTile(position, null);
            return true;
        }
    }

    public PrefabTile GetTile(Vector3 position) => GetTile(placeables.WorldToCell(position));

    public PrefabTile GetTile(Vector3Int position) => placeables.GetTile(position) as PrefabTile;

    public IEnumerable<T> GetPlaceablesOfType<T>() where T : Placeable =>
        placeds.Select(p => p as T).Where(p => p != null);
}
