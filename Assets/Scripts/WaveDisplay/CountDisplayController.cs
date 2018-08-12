using UnityEngine;

public class CountDisplayController : MonoBehaviour
{
    public float speed = 20f;

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.localPosition.y <= 0)
        {
            Debug.Log("spawn monsters!");
            Destroy(gameObject, .5f);

            // somehow from the wave:
            // GameObject prefab = wave.Asset.monsterPrefab;	
            // and then spawn the monsters... 
        }
    }
}
