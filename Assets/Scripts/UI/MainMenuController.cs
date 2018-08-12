using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    private const string JoinButton = "Jump";
    private const string LeaveButton = "Remove";
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
            if (Input.GetButtonDown($"{JoinButton}_{i}"))
            {
                if (!Settings.JoinedPlayers.Any(p => p.PlayerNumber == i))
                {
                    if (Settings.JoinedPlayers.Count >= colours.Length) { continue; }

                    var colour = colours.First(c => Settings.JoinedPlayers.All(p => c != p.Colour));
                    Settings.AddPlayer(colour, i);
                    if (i - 1 < standaloneInputs.Length) { standaloneInputs[i - 1].enabled = true; }

                    var player = Instantiate(playerUiPrefab, playersParent);
                    player.colour = colour;
                    player.playerNumber = i;
                }
                else
                {
                    // A player that already joined pressed the button => start the game
                    StartGame();
                }
            }

            // Player leave the ready state
            if (Input.GetButtonDown($"{LeaveButton}_{i}"))
            {
                var joinedPlayer = Settings.JoinedPlayers.FirstOrDefault(p => p.PlayerNumber == i);
                if(joinedPlayer != null)
                {
                    Settings.RemovePlayer(joinedPlayer);
                    var playerUi = playersParent.GetComponentsInChildren<PlayerUiJoined>().FirstOrDefault(p => p.playerNumber == i);
                    if(playerUi != null) { Destroy(playerUi.gameObject); }
                    if (i - 1 < standaloneInputs.Length) { standaloneInputs[i - 1].enabled = false; }
                }
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
            SceneManager.LoadScene("MainScene");
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
