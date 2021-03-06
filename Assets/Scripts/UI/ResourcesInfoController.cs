﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Resources;

public class ResourcesInfoController : MonoBehaviour
{
    public GameObject resourceDisplayPrefab;

    public Sprite energyIcon;
    public Color energyColor;
    public Sprite oxygenIcon;	
    public Color oxygenColor;
    public Sprite constructionMaterialIcon;
    public Color constructionMaterialColor;
    public Sprite bloodIcon;
    public Color bloodColor;	
	
    public int xDistance = 70;
    public int yDistance = 0;

    public ResourceManager resourceManager;

    private GameObject energyDisplay;
    private GameObject oxygenDisplay;
    private GameObject materialDisplay;
    private GameObject bloodDisplay;

    private Text energyText;
    private Text oxygenText;
    private Text materialText;
    private Text bloodText;

	
    // Use this for initialization
    void Start ()
    {           
        energyDisplay = AddDisplay("Energy", energyIcon, energyColor, 0, 0);
        oxygenDisplay = AddDisplay("Oxygen", oxygenIcon, oxygenColor, 1, 0);
        materialDisplay = AddDisplay("Construction material", constructionMaterialIcon, constructionMaterialColor, 2, 0);
        bloodDisplay = AddDisplay("Blood", bloodIcon, bloodColor, 3, 0);
        
        energyText = energyDisplay.GetComponentInChildren<Text>();
        oxygenText = oxygenDisplay.GetComponentInChildren<Text>();
        materialText = materialDisplay.GetComponentInChildren<Text>();
        bloodText = bloodDisplay.GetComponentInChildren<Text>();

    }

    private GameObject AddDisplay(string name, Sprite icon, Color color, int xPosition, int yPosition)
    {
        GameObject display = Instantiate(resourceDisplayPrefab, 
            transform.position + Vector3.right * xPosition * xDistance + Vector3.down * yPosition * yDistance,
            resourceDisplayPrefab.transform.rotation,
            transform
        );
        display.name = name;

        Image image = display.GetComponentInChildren<Image>();
        image.sprite = icon;

        Text text = display.GetComponentInChildren<Text>();
        text.text = "0 / 0";
        text.color = color;

        return display;
    }
	
    private void Update ()
    {
        energyText.text = $"{(float) resourceManager.EnergyAvailable:0} / {(float)resourceManager.EnergyCapacity:0}";
        oxygenText.text = $"{(float)resourceManager.OxygenAvailable:0} / {(float)resourceManager.OxygenCapacity:0}";
        materialText.text = $"{(float)resourceManager.ConstructionMaterialAvailable:0} / {(float)resourceManager.ConstructionMaterialCapacity:0}";
        bloodText.text = $"{(float)resourceManager.BloodAvailable:0} / {(float)resourceManager.BloodCapacity:0}";
    }
    
}
