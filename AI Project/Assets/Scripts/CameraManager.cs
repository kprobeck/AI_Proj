using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField] private Camera worldCamera;
    [SerializeField] private Camera personCamera;


	// Use this for initialization
	void Start () {
        worldCamera.enabled = false;
        personCamera.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            worldCamera.enabled = !worldCamera.enabled;
            personCamera.enabled = !personCamera.enabled;
        }
    }
}
