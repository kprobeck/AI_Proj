using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField] private Camera worldCamera;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera topDownCamera;
    private List<Camera> cameras;
    private int enabledIndex;

    // Use this for initialization
    void Start () {
        worldCamera.enabled = false;
        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        topDownCamera.enabled = false;

        cameras = new List<Camera> { worldCamera, firstPersonCamera, thirdPersonCamera, topDownCamera };
        enabledIndex = 1; // Index of first person camera
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameras[getNextCamera()].enabled = true;
            resetOtherCameras();
        }
    }

    int getNextCamera()
    {
        enabledIndex++; // increment index
        if (enabledIndex < cameras.Count)
        {
            return enabledIndex; // return index if still in bounds
        }
        else
        {
            enabledIndex = 0; // if out of bounds, reset to index 0
            return enabledIndex;
        }
    }

    void resetOtherCameras()
    {
        List<Camera> otherCameras = new List<Camera>(cameras);
        otherCameras.RemoveAt(enabledIndex);

        foreach (Camera c in otherCameras)
        {
            c.enabled = false;
        }
    }
}
