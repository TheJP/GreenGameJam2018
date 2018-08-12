using Resources;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Tile that contains a prefab.
/// This should only be placed on a tilemap that does not have a renderer
/// to avoid displaying the prefab and tile sprite at the same time.
/// </summary>
[CreateAssetMenu]
public class PrefabTile : TileBase
{
    [Tooltip("The sprite of tile in the palette")]
    public Sprite Sprite;

    [Tooltip("The gameobject to spawn")]
    public GameObject Prefab;

    [Tooltip("The Costs to build this Tile")]
    public ConstructionMaterial BuildingCosts;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = Sprite;
        tileData.gameObject = Prefab;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject gameObject)
    {
        if (gameObject != null) { gameObject.transform.position += new Vector3(0.5f, 0.5f, 0f); }
        return true;
    }
}
