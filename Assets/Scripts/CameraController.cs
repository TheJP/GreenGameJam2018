using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float minimalCameraSize = 5;

    private Camera camera;

    private void Awake() => camera = GetComponent<Camera>();


}
