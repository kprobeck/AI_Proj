using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject[] Flock;
    private Vector3 flockDirection;
    Vector3 centerOfFlock;
    [SerializeField]
    GameObject textBox;
    private string textInfo;

    public float flockSeekWeight = 50.0f;
    public float flockAvoidObstacleWeight = 100.0f;
    public float flockSeperationWeight = 25.0f;
    public float flockCohesionWeight = 25.0f;

    // Use this for initialization
    void Start () {
        Flock = GameObject.FindGameObjectsWithTag("flocker");
        textBoxEdit();
    }
	
	// Update is called once per frame
	void Update () {
        calcFlockDirection();
        calcCenterOfFlock();
        // TODO: Implement keyboard commands to modify flocking behaviors/weights
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

    private void textBoxEdit()
    {
        textInfo = "";
        textInfo += "Seek Weight:    " + flockSeekWeight + "\n";
        textInfo += "Obstacle Avoid: " + flockAvoidObstacleWeight + "\n";
        textInfo += "Seperation:     " + flockSeperationWeight + "\n";
        textInfo += "Cohesion:       " + flockCohesionWeight + "\n";
        textBox.GetComponent<Text>().text = textInfo;
    }
}
