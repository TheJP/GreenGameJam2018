using UnityEngine;

public abstract class Placeable : MonoBehaviour
{
    protected TileController TileController { get; private set; }

    protected virtual void Start()
    {
        TileController = FindObjectOfType<TileController>();
        TileController?.AddPlaceable(this);
    }

    protected virtual void OnDestroy() => TileController?.RemovePlaceable(this);
}
