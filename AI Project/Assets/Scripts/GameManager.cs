using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject[] Flock;
    private Vector3 flockDirection;
    Vector3 centerOfFlock;

    // Use this for initialization
    void Start () {
        Flock = GameObject.FindGameObjectsWithTag("flocker");
    }
	
	// Update is called once per frame
	void Update () {
        calcFlockDirection();
        calcCenterOfFlock();
        // TODO: Implement keyboard commands to modify flocking behaviors/weights
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
}
