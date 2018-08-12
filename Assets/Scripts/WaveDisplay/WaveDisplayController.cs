using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class WaveDisplayController : MonoBehaviour
{

    public CountDisplayController enemyCountDisplayPrefab;
    public int distance;
    public int numberOfPlaces;

    // public GameObject[] m_Displays;

    private Wave[] waves;
    private int offsetPlaces;       // zeitabhängig..., besser lösen

    private RectTransform rectTransform;

    private void Awake() => rectTransform = GetComponent<RectTransform>();

    public void AddWaveEntry(Wave wave)
    {
        var spawnPosition = new Vector3(transform.position.x, rectTransform.rect.yMax, enemyCountDisplayPrefab.transform.position.z);
        var display = Instantiate(enemyCountDisplayPrefab, spawnPosition, enemyCountDisplayPrefab.transform.rotation, transform);
        display.name = $"Wave: {wave.Count} of type {wave.Asset.monsterName}";

        Text text = display.GetComponentInChildren<Text>();
        text.text = $"{wave.Count}";
        text.color = wave.Asset.textColor;

        Image image = display.GetComponentInChildren<Image>();
        image.sprite = wave.Asset.icon;
    }
}
