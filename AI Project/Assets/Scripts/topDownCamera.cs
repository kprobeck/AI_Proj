using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class topDownCamera : MonoBehaviour {
    [SerializeField]
    private float WASDmove = 1.0f;
    [SerializeField]
    private float zoom = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xDisplace = 0f;
        float yDisplace = -Input.GetAxis("Mouse ScrollWheel") * zoom;
        float zDisplace = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            zDisplace -= WASDmove;
        }
        if (Input.GetKey(KeyCode.A))
        {
            xDisplace += WASDmove;
        }
        if (Input.GetKey(KeyCode.S))
        {
            zDisplace += WASDmove;
        }
        if (Input.GetKey(KeyCode.D))
        {
            xDisplace -= WASDmove;
        }

        transform.position = new Vector3(transform.position.x + xDisplace, transform.position.y + yDisplace, transform.position.z + zDisplace);
    }
}
