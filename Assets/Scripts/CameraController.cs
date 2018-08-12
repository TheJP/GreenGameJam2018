using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Tooltip("Player spawner that contains a list of active players")]
    public PlayerSpawner playerSpawner;

    [Tooltip("Minimal size that the camera uses (prevents extreme zoom in)")]
    public float minimalCameraSize = 5;

    [Tooltip("Border that gets shown additionally to the left and right of the players")]
    public float shownBorder = 3f;

    private Camera moveableCamera;

    private void Awake() => moveableCamera = GetComponent<Camera>();

    private void Update()
    {
        var players = playerSpawner.Players;

        // Calculate bounds of players
        var first = players.First().transform.position;
        var bounds = players.Skip(1).Aggregate(
            new { Min = new Vector2(first.x, first.y), Max = new Vector2(first.x, first.y) },
            (current, player) => new
            {
                Min = new Vector2(Mathf.Min(current.Min.x, player.transform.position.x), Mathf.Min(current.Min.y, player.transform.position.y)),
                Max = new Vector2(Mathf.Max(current.Max.x, player.transform.position.x), Mathf.Max(current.Max.y, player.transform.position.y))
            }
        );

        var centre = (bounds.Max + bounds.Min) / 2;
        moveableCamera.transform.position = new Vector3(centre.x, centre.y, moveableCamera.transform.position.z);

        var radius = (bounds.Max - bounds.Min) / 2;
        var widthSize = radius.x / moveableCamera.aspect;
        moveableCamera.orthographicSize = Mathf.Max(minimalCameraSize, widthSize + shownBorder, radius.y + shownBorder);
    }
}
