using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class PrefabTile : UnityEngine.Tilemaps.TileBase
{
    [Tooltip("The sprite of tile in the palette")]
    public Sprite Sprite;

    [Tooltip("The gameobject to spawn")]
    public GameObject Prefab;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = null;
        if (Prefab) tileData.gameObject = Prefab;
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        go.transform.position += Vector3.up * 0.5f + Vector3.right * 0.5f;
        return true;
    }
}
