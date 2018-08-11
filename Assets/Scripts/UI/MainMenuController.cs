using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    private const string JoinButton = "Jump";
    private const int ConfiguredInputs = 5;

    [Tooltip("Prefab that is added to the main menu when a player joins")]
    public PlayerUiJoined playerUiPrefab;

    [Tooltip("Panel to which all player ui prefabs are added")]
    public RectTransform playersParent;

    [Tooltip("Player colours that are available")]
    public Color[] colours;

    [Tooltip("Text that allows to represent status messages")]
    public Text statusText;

    private StandaloneInputModule[] standaloneInputs;

    private void Start() => standaloneInputs = FindObjectOfType<StandaloneInput>().standaloneInputModules;

    private void Update()
    {
        for (int i = 1; i <= ConfiguredInputs; ++i)
        {
            if (Input.GetButtonDown($"{JoinButton}_{i}") &&
                !Settings.JoinedPlayers.Any(p => p.PlayerNumber == i) &&
                Settings.JoinedPlayers.Count < colours.Length)
            {
                var colour = colours[Settings.JoinedPlayers.Count];
                Settings.AddPlayer(colour, i);
                if(i - 1 < standaloneInputs.Length) { standaloneInputs[i - 1].enabled = true; }

                var player = Instantiate(playerUiPrefab, playersParent);
                player.colour = colour;
                player.playerNumber = i;
            }
        }
    }

    public void StartGame()
    {
        if (Settings.JoinedPlayers.Count < 1)
        {
            statusText.text = "Can't start the game without players";
        }
        else
        {
            statusText.text = "";
            // TODO: Load main scene
        }
    }

    public void EditSettings()
    {
        // TODO: Add settings page (for e.g. sound)
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
