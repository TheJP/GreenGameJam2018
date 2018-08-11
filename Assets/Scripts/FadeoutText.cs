using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeoutText : MonoBehaviour
{
    [Tooltip("Speed in units per second with which the text flies upwards")]
    public float flyUpSpeed = 2f;

    [Tooltip("Duration in seconds that the animation takes to finish")]
    public float fadeoutDuration = 2f;

    private float startTime;
    private Color startColour;
    private Color targetColour;

    public TextMesh TextMesh { get; private set; }

    private void Awake() => TextMesh = GetComponentInChildren<TextMesh>();

    private void Start()
    {
        startTime = Time.time;
        startColour = TextMesh.color;
        targetColour = new Color(startColour.r, startColour.g, startColour.b, 0f);
        Destroy(gameObject, fadeoutDuration);
    }

    private void Update()
    {
        transform.position += Vector3.up * flyUpSpeed * Time.deltaTime;
        TextMesh.color = Color.Lerp(startColour, targetColour, (Time.time - startTime) / fadeoutDuration);
    }
}
