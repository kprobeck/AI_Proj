using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject[] Flock;
    private Vector3 flockDirection;
    Vector3 centerOfFlock;
    [SerializeField]
    GameObject textBoxL;
    [SerializeField]
    GameObject textBoxR;
    private string textInfo;

    public float flockSeekWeight = 50.0f;
    public float flockAvoidObstacleWeight = 100.0f;
    public float flockSeperationWeight = 25.0f;
    public float flockCohesionWeight = 25.0f;

    public float flockSeekWeightOriginal;
    public float flockAvoidObstacleWeightOriginal;
    public float flockSeperationWeightOriginal;
    public float flockCohesionWeightOriginal;

    // Use this for initialization
    void Start () {
        Flock = GameObject.FindGameObjectsWithTag("flocker");
        flockSeekWeightOriginal = flockSeekWeight;
        flockAvoidObstacleWeightOriginal = flockAvoidObstacleWeight;
        flockSeperationWeightOriginal = flockSeperationWeight;
        flockCohesionWeightOriginal = flockCohesionWeight;
        textBoxEdit();
    }
	
	// Update is called once per frame
	void Update () {
        calcFlockDirection();
        calcCenterOfFlock();
        keyBoardInputs(1.0f, 200.0f);
        textBoxEdit();
    }

    public Vector3 FlockDirection
    {
        get { return flockDirection; }
    }

    public Vector3 CenterOfFlock
    {
        get { return centerOfFlock; }
        set { centerOfFlock = value; }
    }

    public void calcFlockDirection()
    {
        Vector3 desiredVelocity = Vector3.zero;
        foreach (GameObject g in Flock)
        {
            desiredVelocity += g.GetComponent<Flockers>().Velocity;
        }
        //compute desiredVelocity: normalize the sum from step 1,multiply by maxSpeed
        //compute the steering force: desiredVelocity-currentVelocity

        flockDirection = desiredVelocity.normalized;
    }

    private void calcCenterOfFlock()
    {
        Vector3 center = Vector3.zero;
        foreach(GameObject f in Flock)
        {
            center += f.GetComponent<Flockers>().pos;
        }
        center /= Flock.Length;
        centerOfFlock = center;
    }

    private void keyBoardInputs(float minValue, float maxValue)
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            flockSeekWeight = flockSeekWeightOriginal;
            flockAvoidObstacleWeight = flockAvoidObstacleWeightOriginal;
            flockSeperationWeight = flockSeperationWeightOriginal;
            flockCohesionWeight = flockCohesionWeightOriginal;
            return;
        }
        //keyboard commands to modify flocking behaviors/weights
        if (Input.GetKey(KeyCode.Alpha1) && flockSeekWeight > minValue)
        {
            flockSeekWeight--;
            //Debug.Log("Key 1");
        }
        if (Input.GetKey(KeyCode.Alpha2) && flockSeekWeight < maxValue)
        {
            flockSeekWeight++;
            //Debug.Log("Key 2");
        }

        if (Input.GetKey(KeyCode.Alpha3) && flockAvoidObstacleWeight > minValue)
        {
            flockAvoidObstacleWeight--;
            //Debug.Log("Key 3");
        }
        if (Input.GetKey(KeyCode.Alpha4) && flockAvoidObstacleWeight < maxValue)
        {
            flockAvoidObstacleWeight++;
            //Debug.Log("Key 4");
        }

        if (Input.GetKey(KeyCode.Alpha5) && flockSeperationWeight > minValue)
        {
            flockSeperationWeight--;
            //Debug.Log("Key 5");
        }
        if (Input.GetKey(KeyCode.Alpha6) && flockSeperationWeight < maxValue)
        {
            flockSeperationWeight++;
            //Debug.Log("Key 6");
        }

        if (Input.GetKey(KeyCode.Alpha7) && flockCohesionWeight > minValue)
        {
            flockCohesionWeight--;
            //Debug.Log("Key 7");
        }
        if (Input.GetKey(KeyCode.Alpha8) && flockCohesionWeight < maxValue)
        {
            flockCohesionWeight++;
            //Debug.Log("Key 8");
        }
    }

    private void textBoxEdit()
    {
        textInfo = "";

        textInfo += "Team Color Selected: ";
        if (GetComponent<WorldInput>().isPlacingRedTeam)
        {
            textBoxL.GetComponent<Text>().color = Color.red;
            textInfo += "Red";
        }
        else
        {
            textBoxL.GetComponent<Text>().color = Color.green;
            textInfo += "Green";
        }

        textBoxR.GetComponent<Text>().text = "WASD to move Camera \n Mouse Wheel to Zoom \n  'Q' to switch team to place \n 1, 2, 3, 4 to place unit with that strength at mouse location \n 'R' to clear units and map \n Enter/Space to generate influence map \n Right-Click on a Unit to remove it";

        textBoxL.GetComponent<Text>().text = textInfo;
    }
}
