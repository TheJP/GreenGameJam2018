using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiJoined : MonoBehaviour
{
    [Tooltip("Colour of this player")]
    public Color colour;

    [Tooltip("Number of this player")]
    public int playerNumber;

    [Tooltip("Image that gets a tint from the player colour")]
    public Image imageTint;

    private void Start() => imageTint.color = colour;
}
