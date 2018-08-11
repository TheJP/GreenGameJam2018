using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StandaloneInput : MonoBehaviour
{
    [Tooltip("Standalone inputs that will get activated when their respective player joins")]
    public StandaloneInputModule[] standaloneInputModules;
}
