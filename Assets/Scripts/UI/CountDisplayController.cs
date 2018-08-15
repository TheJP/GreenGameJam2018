using System;
using UnityEngine;

public class CountDisplayController : MonoBehaviour
{

    public event Action SpawnWave;

    private float startTime;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool invoked = false;
    private float duration = 0.001f;

    public float Duration
    {
        get { return duration; }
        set { duration = Mathf.Max(0.001f, value); }
    }

    private void Start()
    {
        startTime = Time.time;
        startPosition = transform.position;
        targetPosition = new Vector3(transform.position.x, -100, transform.position.z); // TODO: Replace 100 with `height / 2`
    }

    private void Update()
    {
        transform.position = Vector3.LerpUnclamped(startPosition, targetPosition, (Time.time - startTime) / Duration);
        if (transform.localPosition.y <= 0)
        {
            if (!invoked)
            {
                invoked = true;
                SpawnWave?.Invoke();
            }
            Destroy(gameObject, .5f);
        }
    }
}
